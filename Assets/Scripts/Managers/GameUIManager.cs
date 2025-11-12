using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;       // Para Coroutines
using System.Collections.Generic; // Para Listas
using UnityEngine.Networking; // Para UnityWebRequest
using TMPro;                  // Para los Dropdowns
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [Header("Conexión API")]
    public string apiUrl = "http://127.0.0.1:5001/api/v1/simulate";
    public Button buttonEnviar; // Botón para enviar el turno al modelo de IA

    [Header("Paneles Principales")]
    public GameObject panelConsumidor;      // Panel central con la barra de aceptación 
    public GameObject panelAtributos;     // Panel de hexágonos 
    public GameObject panelExploracion;   // (Aún no diseñado, pero se lo prepara)
    public GameObject panelPublicidad;    // (Aún no diseñado, pero se lo prepara)
    public GameObject panelPerfil;        // Panel de perfil 

    [Header("Escenas de Salida")]
    public string escenaMainMenu = "MainMenu";
    public string escenaReporte = "EndGameScene";

    void Start()
    {
        // Al empezar el juego, solo se muestra el panel principal del consumidor
        ShowPanelConsumidor();
    }

    // FUNCIONES DE NAVEGACIÓN PRINCIPAL

    // Función para ocultar todos los paneles intercambiables
    private void OcultarTodosLosPaneles()
    {
        // Si el panel existe, lo desactiva
        if (panelConsumidor != null) panelConsumidor.SetActive(false);
        if (panelAtributos != null) panelAtributos.SetActive(false);
        if (panelExploracion != null) panelExploracion.SetActive(false);
        if (panelPublicidad != null) panelPublicidad.SetActive(false);
        if (panelPerfil != null) panelPerfil.SetActive(false);
    }

    // Funciones que llamarán a los botones
    public void ShowPanelConsumidor()
    {
        OcultarTodosLosPaneles();
        if (panelConsumidor != null) panelConsumidor.SetActive(true);
    }

    public void ShowPanelAtributos()
    {
        OcultarTodosLosPaneles();
        if (panelAtributos != null) panelAtributos.SetActive(true);
    }

    public void ShowPanelExploracion()
    {
        OcultarTodosLosPaneles();
        if (panelExploracion != null) panelExploracion.SetActive(true);
    }

    public void ShowPanelPublicidad()
    {
        OcultarTodosLosPaneles();
        if (panelPublicidad != null) panelPublicidad.SetActive(true);
    }

    public void ShowPanelPerfil()
    {
        OcultarTodosLosPaneles();
        if (panelPerfil != null) panelPerfil.SetActive(true);
    }

    // FUNCIONES DE BOTONES SUPERIORES
    public void BotonPausa()
    {
        // Time.timeScale = 0; // (Lógica de pausa)
        Debug.Log("Juego Pausado");
    }

    public void BotonInicio()
    {
        // Esta función llevará de vuelta al menú principal
        // O, a su vez, puede llamar a ShowPanelConsumidor()
        ShowPanelConsumidor();
    }

    // FUNCIONES DE FIN DE JUEGO
    public void EnviarTurno()
    {
        // Se desactiva el botón en una primera instancia
        if (buttonEnviar != null) buttonEnviar.interactable = false;

        // Inicia la corutina que hace la llamada a la API
        StartCoroutine(SimularTurnoCoroutine());

    }

    // 2. Esta es la corutina que hace la llamada web
    IEnumerator SimularTurnoCoroutine()
    {
        Debug.Log("Iniciando simulación... preparando datos...");

        // 1. PREPARAR EL CONTEXTO SIMULADO
        // (En el caso real, estos datos vienen del estado del juego)
        ContextoRequest contextoSimulado = new ContextoRequest
        {
            macroentorno = new List<ContextoFactor>
            {
                new ContextoFactor { detalle = "Nueva ley de etiquetado", importancia = "Media" }
            },
            perfilConsumidor = new List<ContextoFactor>
            {
                new ContextoFactor { detalle = "Influencia de redes sociales", importancia = "Alta" },
                new ContextoFactor { detalle = "Preferencia por productos orgánicos (eco)", importancia = "Alta" }
            }
        };

        // 2. PREPARAR LA DECISIÓN
        // Se usan los mismos datos del 'test.json' para
        // que la API de Python responda bien.
        DecisionRequest decisionSimulada = new DecisionRequest
        {
            // Asumimos que el jugador "eligió":
            idProducto = 2,  // "Empaque de plástico reciclado"
            idPrecio = 3,    // "Precio de Penetración"
            idPlaza = 5,     // (No está en tu UI, pero la IA lo espera)
            idPromocion = 1  // "Campaña en TikTok"
        };

        // 3. COMBINAR DATOS EN LA SOLICITUD FINAL
        SimulationRequest requestData = new SimulationRequest
        {
            decision = decisionSimulada,
            contexto = contextoSimulado
        };

        string jsonBody = JsonUtility.ToJson(requestData);
        Debug.Log("Enviando JSON: " + jsonBody);

        // 4. CONFIGURAR Y ENVIAR LA LLAMADA WEB
        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            // 5. MANEJAR LA RESPUESTA
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                // ¡Error!
                Debug.LogError("Error de API: " + request.error);
                Debug.LogError("Respuesta: " + request.downloadHandler.text);

                // Si falla, se activa el botón de nuevo
                if (buttonEnviar != null) buttonEnviar.interactable = true;
            }
            else
            {
                // ¡Éxito!
                string responseJson = request.downloadHandler.text;
                Debug.Log("Respuesta recibida: " + responseJson);

                SimulationResponse response = JsonUtility.FromJson<SimulationResponse>(responseJson);

                // 6. GUARDAR DATOS REALES
                GameState.resultadoJuego = "HAS GANADO"; // (O "PERDISTE" si puntaje < 800)
                GameState.ultimoTurno = 3; // (Aún por default)
                GameState.presupuestoRestante = 550; // (Aún por default)

                // DATOS REALES DE LA IA
                GameState.nivelAceptacion = response.nivelAceptacion / 100.0f;
                GameState.puntaje = response.puntaje;

                // (No se tiene % de perfil, así que se lo deja por default)
                GameState.nivelPerfil = 0.30f; 

                // 7. CARGAR ESCENA DE REPORTE
                SceneManager.LoadScene(escenaReporte);
            }
        }
    }
    public void BotonIrAMenu()
    {
        // Time.timeScale = 1; // (Asegurarse de quitar la pausa)
        SceneManager.LoadScene(escenaMainMenu);
    }
}