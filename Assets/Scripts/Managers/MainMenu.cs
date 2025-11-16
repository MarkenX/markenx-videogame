using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [Header("Botones de la Escena")]
    public Button buttonIniciar;
    public Button buttonSalir; 

    [Header("Datos de Prueba")]
    // Este ID será reemplazado por el que venga del portal web,
    public string idAsignacionPrueba = "ASIGNACION_1"; 

    // Un texto para mostrar el estado
    //public TextMeshProUGUI textoEstado; 

    void Start()
    {
        if (GameSceneManager.Instance == null)
        {
            Debug.LogError("¡ERROR! No se encontró el 'GameSceneManager'");
            if (buttonIniciar != null) buttonIniciar.interactable = false;
            //if (textoEstado != null) textoEstado.text = "Error: GameSceneManager no encontrado.";
            return;
        }

        // 2. Asignar las funciones a los botones
        if (buttonIniciar != null)
        {
            buttonIniciar.interactable = true;
            buttonIniciar.onClick.AddListener(OnIniciarPartida);
            //if (textoEstado != null) textoEstado.text = "Listo para iniciar.";
        }

        if (buttonSalir != null)
        {
            buttonSalir.onClick.AddListener(OnSalirDelJuego);
        }
    }

    public void OnIniciarPartida()
    {
        // Desactiva el botón para evitar doble clic
        if (buttonIniciar != null) buttonIniciar.interactable = false;
        //if (textoEstado != null) textoEstado.text = "Cargando reglas desde el backend...";

        // Llama al Manager (que es persistente) para que inicie la carga.
        // El Manager se encargará de llamar al backend y cargar la 'GameScene'.
        GameSceneManager.Instance.IniciarPartida(idAsignacionPrueba);
    }

    public void OnSalirDelJuego()
    {
        if (GameSceneManager.Instance != null)
        {
            GameSceneManager.Instance.QuitGame();
        }
        else
        {
            Application.Quit();
        }
    }
}