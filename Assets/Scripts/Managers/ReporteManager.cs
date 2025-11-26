using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ReporteManager : MonoBehaviour
{
    [Header("UI de Reporte")]
    public TextMeshProUGUI textoResultado;
    public TextMeshProUGUI textoTurno;
    public TextMeshProUGUI textoPresupuesto;
    
    public Slider sliderAceptacion;
    public TextMeshProUGUI textoPorcentajeAceptacion; 
    
    public Slider sliderPerfil;
    public TextMeshProUGUI textoPorcentajePerfil; 
    
    [Header("Botones")]
    public Button botonGrafico;
    public Button botonSalir;

    void Start()
    {
        // DEBUG: Verificamos si llegan los datos
        Debug.Log($"Resultados cargados: Aceptación {GameState.nivelAceptacion}, Presupuesto {GameState.presupuestoRestante}");

        // 1. LEER DATOS
        string resultado = GameState.resultadoJuego;
        int turno = GameState.ultimoTurno;
        int presupuesto = GameState.presupuestoRestante;
        float aceptacion = GameState.nivelAceptacion; // 0.0 a 1.0
        float perfil = GameState.nivelPerfil;         // 0.0 a 1.0

        // 2. ACTUALIZAR UI (Validando nulos para evitar errores a medias)
        if (textoResultado) textoResultado.text = resultado;
        if (textoTurno) textoTurno.text = "TURNOS UTILIZADOS: " + (turno - 1); // Restamos 1 porque el turno avanza al terminar
        if (textoPresupuesto) textoPresupuesto.text = "PRESUPUESTO: $" + presupuesto;
        
        if (sliderAceptacion) sliderAceptacion.value = aceptacion; // El slider va de 0 a 1
        if (textoPorcentajeAceptacion) textoPorcentajeAceptacion.text = (aceptacion * 100).ToString("F0") + "%";

        if (sliderPerfil) sliderPerfil.value = perfil;
        if (textoPorcentajePerfil) textoPorcentajePerfil.text = (perfil * 100).ToString("F0") + "%";
        
        // 3. CONECTAR BOTONES
        if (botonGrafico) 
        {
            botonGrafico.onClick.RemoveAllListeners();
            botonGrafico.onClick.AddListener(() => CambiarEscena("GraficoScene"));
        }

        if (botonSalir) 
        {
            botonSalir.onClick.RemoveAllListeners();
            botonSalir.onClick.AddListener(() => CambiarEscena("MainMenu"));
        }
    }

    void CambiarEscena(string escena)
    {
        if (SceneTransitionManager.Instance != null)
            SceneTransitionManager.Instance.CargarEscena(escena);
        else
            SceneManager.LoadScene(escena);
    }
}