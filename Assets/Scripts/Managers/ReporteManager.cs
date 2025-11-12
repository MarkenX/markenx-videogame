using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReporteManager : MonoBehaviour
{
    [Header("UI de Reporte")]
    public TextMeshProUGUI textoResultado;
    public TextMeshProUGUI textoTurno;
    public TextMeshProUGUI textoPresupuesto;
    public TextMeshProUGUI textoPorcentajeAceptacion;
    public Slider sliderAceptacion;
    public TextMeshProUGUI textoPorcentajePerfil;
    public Slider sliderPerfil;

    void Start()
    {
        // 1. Leer los datos de GameState
        string resultado = GameState.resultadoJuego;
        int turno = GameState.ultimoTurno;
        int presupuesto = GameState.presupuestoRestante;
        float aceptacion = GameState.nivelAceptacion;
        float perfil = GameState.nivelPerfil;

        // 2. Aplicar esos datos a la UI
        if (textoResultado != null) textoResultado.text = resultado;
        if (textoTurno != null) textoTurno.text = "TURNO: " + turno.ToString();
        if (textoPresupuesto != null) textoPresupuesto.text = "PRESUPUESTO: " + presupuesto.ToString() + "$";
        
        if (sliderAceptacion != null) sliderAceptacion.value = aceptacion;
        if (sliderPerfil != null) sliderPerfil.value = perfil;

        // 3. (Opcional) Actualizar los textos de porcentaje
        if (textoPorcentajeAceptacion != null)
            textoPorcentajeAceptacion.text = "ACEPTACIÓN: " + (aceptacion * 100).ToString("F0") + "%";
        
        if (textoPorcentajePerfil != null)
            textoPorcentajePerfil.text = "PERFIL: " + (perfil * 100).ToString("F0") + "%";
    }

}