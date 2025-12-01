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
        // 1. DIAGNÓSTICO DE DATOS
        Debug.Log($"[REPORTE] Cargando datos de GameState...");
        Debug.Log($"[REPORTE] Resultado: {GameState.resultadoJuego}");
        Debug.Log($"[REPORTE] Presupuesto Final: {GameState.presupuestoRestante}");
        Debug.Log($"[REPORTE] Aceptación Final: {GameState.nivelAceptacion}");

        // 2. LEER DATOS
        string resultado = GameState.resultadoJuego;
        int turno = GameState.ultimoTurno;
        int presupuesto = GameState.presupuestoRestante;
        float aceptacion = GameState.nivelAceptacion; 
        float perfil = GameState.nivelPerfil;       

        // 3. ACTUALIZAR UI (Con reporte de errores si falta algo)
        if (textoResultado != null) textoResultado.text = resultado;

        if (textoTurno != null) textoTurno.text = "TURNOS: " + (turno - 1);

        if (textoPresupuesto != null) textoPresupuesto.text = "PRESUPUESTO: $" + presupuesto;
        
        if (sliderAceptacion != null) sliderAceptacion.value = aceptacion;

        if (textoPorcentajeAceptacion != null) textoPorcentajeAceptacion.text = "Aceptación: " + (aceptacion * 100).ToString("F0") + "%";
        
        if (sliderPerfil != null) sliderPerfil.value = perfil;
        if (textoPorcentajePerfil != null) textoPorcentajePerfil.text = "Perfil: " + (perfil * 100).ToString("F0") + "%";
        
        // 4. CONECTAR BOTONES
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