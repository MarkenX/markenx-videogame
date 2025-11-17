using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Networking;
using System.Linq;

public class GameSceneManager : MonoBehaviour
{
    // SINGLETON
    public static GameSceneManager Instance { get; private set; }

    // ESTADO DEL JUEGO
    private int presupuestoActual;
    private float aceptacionActual;
    private int turnoActual;
    private string noticiaActual;
    private List<EscenarioPerfilSubfactor> perfilActualizado; // Perfil que puede ser modificado por eventos
    private List<Turno> historialTurnos = new List<Turno>();
    private List<Accion> accionesCompradasEsteTurno = new List<Accion>();

    // LÓGICA DE ÁRBOL DE HABILIDADES Y PERFIL
    private HashSet<int> accionesDesbloqueadas = new HashSet<int>();
    private HashSet<int> subfactoresDescubiertos = new HashSet<int>();

    // REGLAS DEL JUEGO (CARGADAS AL INICIO)
    private PartidaDataPayload partidaData; 

    // CONEXIÓN BACKEND
    private string apiUrl_CargarPartida = "http://URL_BACKEND/api/partida/iniciar";
    private string apiUrl_GuardarPartida = "http://URL_BACKEND/api/partida/guardar";

    [Header("Configuración de Juego")]
    [Range(0f, 1f)]
    public float probabilidadEventoNoticia = 0.25f;


    void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
    }

    // 1. LLAMADO DESDE MainMenu
    public void IniciarPartida(string idAsignacion)
    {
        StartCoroutine(CargarReglasCoroutine(idAsignacion));
    }

    // 2. CORUTINA DE CARGA
    IEnumerator CargarReglasCoroutine(string idAsignacion)
    {
        string urlConParams = $"{apiUrl_CargarPartida}?idAsignacion={idAsignacion}";
        using (UnityWebRequest request = UnityWebRequest.Get(urlConParams))
        {
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success) {
                Debug.LogError("Error al cargar reglas: " + request.error);
            } else {
                string jsonResponse = request.downloadHandler.text;
                partidaData = JsonUtility.FromJson<PartidaDataPayload>(jsonResponse);

                // 3. INICIALIZAR EL ESTADO DEL JUEGO
                presupuestoActual = partidaData.presupuestoInicial;
                aceptacionActual = 0;
                turnoActual = 1;
                noticiaActual = "El mercado está estable.";
                perfilActualizado = new List<EscenarioPerfilSubfactor>(partidaData.perfilConsumidor);
                historialTurnos.Clear();
                accionesCompradasEsteTurno.Clear();

                // 4. INICIALIZAR LÓGICA DE ÁRBOL Y PERFIL
                accionesDesbloqueadas.Clear();
                subfactoresDescubiertos.Clear();
                foreach (var accion in partidaData.accionesDisponibles.Where(a => !a.esBloqueadaInicialmente)) {
                    accionesDesbloqueadas.Add(accion.idAccion);
                }
                foreach (var factor in partidaData.perfilConsumidor.Where(p => p.esVisibleInicialmente)) {
                    subfactoresDescubiertos.Add(factor.idSubfactor);
                }

                // 5. CARGAR LA ESCENA DEL JUEGO
                SceneManager.LoadScene("GameScene");
            }
        }
    }

    // --- MÉTODOS PÚBLICOS (Llamados por la UI) ---

    public PartidaDataPayload GetPartidaData() => partidaData;
    public int GetPresupuestoActual() => presupuestoActual;
    public float GetAceptacionActual() => aceptacionActual;
    public int GetTurnoActual() => turnoActual;
    public string GetNoticiaActual() => noticiaActual;
    public HashSet<int> GetSubfactoresDescubiertos() => subfactoresDescubiertos;
    
    // LOGICA ARBOL DE HABILIDADES
    public bool EstaAccionDesbloqueada(int idAccion)
    {
        return accionesDesbloqueadas.Contains(idAccion);
    }
    
    // LOGICA NIVEL PERFIL
    public float GetNivelPerfil()
    {
        if (partidaData == null || partidaData.perfilConsumidor.Count == 0) return 0f;
        return (float)subfactoresDescubiertos.Count / partidaData.perfilConsumidor.Count;
    }

    // Llamado por el botón "INVERTIR" en GameUIManager
    public bool ComprarAccion(int idAccion)
    {
        if (!EstaAccionDesbloqueada(idAccion)) {
            Debug.LogWarning("Intento de comprar acción BLOQUEADA");
            return false;
        }

        Accion accion = partidaData.accionesDisponibles.Find(a => a.idAccion == idAccion);
        if (accion == null) return false;

        if (presupuestoActual >= accion.costo)
        {
            presupuestoActual -= (int)accion.costo;
            accionesCompradasEsteTurno.Add(accion);

            // LOGICA ARBOL DE HABILIDADES: Desbloquear acciones siguientes
            foreach (var acc in partidaData.accionesDisponibles.Where(a => a.idAccionRequerida == accion.idAccion)) {
                accionesDesbloqueadas.Add(acc.idAccion);
            }

            // LOGICA NIVEL PERFIL: Si es de exploración, descubre un factor
            if (accion.categoria == "EXPLORACION") {
                DescubrirNuevoFactor();
            }

            Debug.Log($"Acción comprada: {accion.nombreAccion}");
            return true;
        }
        
        Debug.Log("¡Presupuesto insuficiente!");
        return false;
    }

    // Llamado por el botón "ENVIAR" en GameUIManager
    public void TerminarTurno()
    {
        float aceptacionCalculada = CalcularAceptacionLocal();
        aceptacionActual = aceptacionCalculada; 

        noticiaActual = ProcesarEventoNoticia(); // LOGICA DE EVENTOS

        GuardarTurnoActual(aceptacionCalculada, noticiaActual);

        turnoActual++;
        accionesCompradasEsteTurno.Clear();

        if (aceptacionActual >= partidaData.aceptacionObjetivo) {
            TerminarPartida("GANASTE");
        } else if (presupuestoActual <= 0 && aceptacionActual < partidaData.aceptacionObjetivo) {
            TerminarPartida("PERDISTE");
        }
    }

    // IA LOCAL)

    private float CalcularAceptacionLocal()
    {
        float aceptacionTotalTurno = 0;
        foreach (Accion accion in accionesCompradasEsteTurno)
        {
            List<AccionSubfactorImpacto> reglasParaEstaAccion = 
                partidaData.reglasImpacto.Where(r => r.idAccion == accion.idAccion).ToList();

            foreach (AccionSubfactorImpacto regla in reglasParaEstaAccion)
            {
                EscenarioPerfilSubfactor perfilFactor = 
                    perfilActualizado.Find(p => p.idSubfactor == regla.idSubfactor);

                if (perfilFactor != null)
                {
                    // A_i * W_i
                    float impactoCalculado = regla.impacto * perfilFactor.peso;
                    aceptacionTotalTurno += impactoCalculado;
                }
            }
        }
        return aceptacionActual + aceptacionTotalTurno;
    }

    // LOGICA NIVEL PERFIL
    private void DescubrirNuevoFactor()
    {
        // Encuentra el primer factor en el perfil que AÚN NO esté en el HashSet de descubiertos
        var factorOculto = partidaData.perfilConsumidor
            .FirstOrDefault(p => !subfactoresDescubiertos.Contains(p.idSubfactor));
            
        if (factorOculto != null)
        {
            subfactoresDescubiertos.Add(factorOculto.idSubfactor);
            Debug.Log($"¡Nuevo factor descubierto! ID: {factorOculto.idSubfactor}");
        }
    }

    // LOGICA DE EVENTOS
    private string ProcesarEventoNoticia()
    {
        if (Random.Range(0f, 1f) <= probabilidadEventoNoticia)
        {
            // ¡Ocurre un evento!
            var evento = partidaData.eventosPosibles[Random.Range(0, partidaData.eventosPosibles.Count)];
            var efectos = partidaData.efectosEventos.Where(e => e.idEvento == evento.idEvento);

            foreach (var efecto in efectos)
            {
                var factorEnPerfil = perfilActualizado.Find(p => p.idSubfactor == efecto.idSubfactor);
                if (factorEnPerfil != null) 
                {
                    // Modifica el peso del perfil *actualizado*
                    factorEnPerfil.peso += efecto.modificadorPeso;
                    Debug.Log($"Evento afectó a {factorEnPerfil.idSubfactor}, nuevo peso: {factorEnPerfil.peso}");
                }
            }
            return evento.detalleNoticia; // Devuelve la noticia para mostrarla
        }
        return "El mercado está estable."; // No pasa nada
    }


    // FIN DE PARTIDA

    private void GuardarTurnoActual(float aceptacion, string noticia)
    {
        Turno turno = new Turno {
            numeroTurno = turnoActual,
            aceptacionTurno = aceptacion,
            eventoNoticiaOcurrido = noticia,
            decisionesAccion = accionesCompradasEsteTurno.Select(a => a.idAccion).ToList()
        };
        historialTurnos.Add(turno);
    }
    
    private void TerminarPartida(string resultado)
    {
        GameState.resultadoJuego = resultado;
        GameState.ultimoTurno = turnoActual;
        GameState.presupuestoRestante = presupuestoActual;
        GameState.nivelAceptacion = aceptacionActual / 100.0f;
        GameState.nivelPerfil = GetNivelPerfil(); // LOGICA NIVEL PERFIL

        partidaData.historialTurnos = historialTurnos;
        StartCoroutine(GuardarPartidaCoroutine());

        SceneManager.LoadScene("EndGameScene");
    }

    IEnumerator GuardarPartidaCoroutine()
    {
        string jsonBody = JsonUtility.ToJson(partidaData);
        using (UnityWebRequest request = new UnityWebRequest(apiUrl_GuardarPartida, "POST"))
        {
            byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success) {
                Debug.LogError("Error al guardar partida: " + request.error);
            } else {
                Debug.Log("¡Partida guardada exitosamente!");
            }
        }
    }

    // Métodos de carga de escena
    public void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
    public void QuitGame() {
        Application.Quit();
    }
}