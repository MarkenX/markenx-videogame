using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using System.Linq; 

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance { get; private set; }

    // ESTADO DEL JUEGO
    private GameContext context; 
    private int turnoActual;
    private float aceptacionActual; 
    
    private string noticiaTituloActual; 
    private string noticiaDetalleActual;

    // UI State
    private HashSet<int> accionesDesbloqueadas = new HashSet<int>();
    private HashSet<int> accionesCompradasTotal = new HashSet<int>();
    private List<AccionInfo> accionesCompradasEsteTurno = new List<AccionInfo>();
    private HashSet<int> subfactoresDescubiertos = new HashSet<int>(); 

    // ESTADO GLOBAL PARA EL MENÚ
    public bool juegoTerminado = false;

    [Header("Configuración")]
    public bool usarModoSimulacion = true;

    void Awake() {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else { Instance = this; DontDestroyOnLoad(gameObject); }
    }

    public void IniciarPartida(string id)
    {
        juegoTerminado = false; // Se reinicia estado al empezar
        if (usarModoSimulacion)
        {
            StartCoroutine(SimularCarga());
        }
    }

    IEnumerator SimularCarga()
    {
        yield return new WaitForSeconds(0.1f);
        context = MockDataFactory.GetEcoGameContext();
        InicializarEstado();
        LoadScene("GameScene");
    }

    void InicializarEstado()
    {
        turnoActual = 1;
        accionesCompradasTotal.Clear();
        accionesCompradasEsteTurno.Clear();
        accionesDesbloqueadas.Clear();
        subfactoresDescubiertos.Clear();
        noticiaTituloActual = "";
        noticiaDetalleActual = "";

        aceptacionActual = 0f;
        //RecalcularAceptacion();

        if (context.AccionesVisuales != null)
        {
            foreach(var acc in context.AccionesVisuales.Where(a => !a.esBloqueadaInicialmente))
                accionesDesbloqueadas.Add(acc.idAccion);
        }
    }

    public bool ComprarAccion(int idAccion)
    {
        if (!accionesDesbloqueadas.Contains(idAccion)) return false;
        if (accionesCompradasTotal.Contains(idAccion)) return false;

        AccionInfo infoVisual = context.AccionesVisuales.Find(a => a.idAccion == idAccion);
        if (infoVisual == null) return false;

        if (context.Presupuesto >= infoVisual.costo)
        {
            context.Presupuesto -= infoVisual.costo;
            accionesCompradasEsteTurno.Add(infoVisual);
            accionesCompradasTotal.Add(idAccion);

            // APLICAR EFECTO AL PRODUCTO
            if (context.ActionsMap.TryGetValue(idAccion, out MarketAction accionLogica))
            {
                accionLogica.Apply(context.Product);
                Debug.Log($"Aplicada acción: {accionLogica.Name}. Nuevo perfil generado.");
            }

            // Desbloquear hijos
            var hijos = context.AccionesVisuales.Where(a => a.idAccionRequerida == idAccion);
            foreach (var h in hijos) accionesDesbloqueadas.Add(h.idAccion);

            // Exploración
            if (idAccion == 40) subfactoresDescubiertos.Add(200); // Encuesta -> Social
            if (idAccion == 41) subfactoresDescubiertos.Add(100); // Focus -> Cultural
            if (idAccion == 42) subfactoresDescubiertos.Add(300); // Estilo -> Personal
            if (idAccion == 43) subfactoresDescubiertos.Add(400); // Motivacion -> Psico

            return true;
        }
        return false;
    }

    public const int MAX_TURNOS = 5;
    public bool TerminarTurno(out string mensajeError)
    {
        // 1. VALIDACIÓN
        if (accionesCompradasEsteTurno.Count == 0)
        {
            mensajeError = "¡Debes comprar al menos una acción antes de enviar el turno!";
            return false; // Cancela el avance
        }

        mensajeError = "";

        // 2. Cálculo Normal
        RecalcularAceptacion();
        
        // Evento Simulado (Turno 2 -> 3)
        if (turnoActual == 2)
        {
            noticiaTituloActual = "El Mundo Se Vuelve Más Verde";
            noticiaDetalleActual = "Impulso global por la sostenibilidad gana fuerza.";
            
            // Efecto en el consumidor
            context.Consumer.Adjust(context.EcoInterest, 0.5f); 
            
            RecalcularAceptacion();
        }
        else
        {
            noticiaTituloActual = "";
            noticiaDetalleActual = "";
        }

        turnoActual++;
        accionesCompradasEsteTurno.Clear();

        // 3. VERIFICAR DERROTA (Por Turnos o Presupuesto)
        // Si se acabó el dinero O se acabaron los turnos y no ganó... PIERDE.
        if (context.Presupuesto <= 0 || turnoActual > MAX_TURNOS)
        {
            // Si al final de los turnos no llegaste al objetivo, pierdes
            if (aceptacionActual < context.AceptacionObjetivo)
            {
                EjecutarFinDeJuego("PERDISTE");
            }
        }

        return true; // Éxito
    }

    private void RecalcularAceptacion()
    {
        aceptacionActual = DistanceMetric.ComputeAcceptance(context.Consumer, context.Product);
        Debug.Log($"Nueva Aceptación Calculada: {aceptacionActual * 100}%");
    }

    public void EjecutarFinDeJuego(string resultado)
    {
        juegoTerminado = true;

        GameState.resultadoJuego = resultado;
        GameState.nivelAceptacion = aceptacionActual;
        GameState.presupuestoRestante = (int)context.Presupuesto;
        GameState.ultimoTurno = turnoActual;
        GameState.nivelPerfil = GetNivelPerfil();
        
        LoadScene("EndGameScene");
    }

    // GETTERS PARA UI
    public List<AccionInfo> GetAccionesDisponibles() => context.AccionesVisuales;
    public int GetPresupuestoActual() => (int)context.Presupuesto;
    public float GetAceptacionActual() => aceptacionActual * 100f; 
    public int GetTurnoActual() => turnoActual;
    public string GetNoticiaTitulo() => noticiaTituloActual;
    public string GetNoticiaDetalle() => noticiaDetalleActual;
    public bool IsAccionDesbloqueada(int id) => accionesDesbloqueadas.Contains(id);
    public bool IsAccionComprada(int id) => accionesCompradasTotal.Contains(id);
    public string GetNombreConsumidor() => context.NombreConsumidor;
    public int GetEdadConsumidor() => context.EdadConsumidor;
    public bool IsFactorDescubierto(int index) => subfactoresDescubiertos.Contains(index);
    
    public HashSet<int> GetSubfactoresDescubiertos() => subfactoresDescubiertos;

    public float GetNivelPerfil() {
        if (context == null || context.Consumer == null) return 0f;
        return (float)subfactoresDescubiertos.Count / 4.0f; 
    }

    public void QuitGame() { Application.Quit(); }

    public void LoadScene(string sceneName)
    {
        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.CargarEscena(sceneName);
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}