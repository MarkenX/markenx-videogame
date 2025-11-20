using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AccionButton : MonoBehaviour
{
    private Accion miAccion;
    private GameUIManager uiManager;

    public Button miBoton;
    //public TextMeshProUGUI miTexto;
    public Image miImagenFondo;

    // Colores (Asegúrate de configurarlos en el Inspector del Prefab si quieres)
    public Color colorBloqueado = Color.gray;
    public Color colorDisponible = Color.white;
    public Color colorComprado = new Color(0.5f, 1f, 0.5f); // Verde claro

    public void Inicializar(Accion accion, GameUIManager manager)
    {
        miAccion = accion;
        uiManager = manager;
        //if(miTexto != null) miTexto.text = accion.nombreAccion;
        
        miBoton.onClick.RemoveAllListeners();
        miBoton.onClick.AddListener(() => uiManager.OnHexagonoClick(miAccion));
    }

    // ESTA ES LA FUNCIÓN QUE DABA ERROR. AHORA ACEPTA 2 ARGUMENTOS.
    public void ActualizarVisual(bool desbloqueada, bool comprada)
    {
        miBoton.interactable = true; // Siempre clickable para ver info

        if (miImagenFondo != null)
        {
            if (comprada)
            {
                miImagenFondo.color = colorComprado;
            }
            else
            {
                miImagenFondo.color = desbloqueada ? colorDisponible : colorBloqueado;
            }
        }
    }

    public int GetAccionID() => miAccion.idAccion;
    public string GetAccionCategoria() => miAccion.categoria;
}