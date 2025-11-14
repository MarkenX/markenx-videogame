using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AccionButton : MonoBehaviour
{
    private Accion miAccion;
    private GameUIManager uiManager;

    [Header("Prefab Components")]
    public Button botonComponent;
    public Image imagenFondo; // El hexágono
    
    //public TextMeshProUGUI textoNombre;
    //public Image icono;

    // Colores para el estado
    public Color colorDesbloqueado = Color.white;
    public Color colorBloqueado = Color.gray;

    // El UIManager llama a esta función cuando crea el botón
    public void Inicializar(Accion accion, GameUIManager manager)
    {
        miAccion = accion;
        uiManager = manager;

        // if (textoNombre != null) textoNombre.text = miAccion.nombreAccion;

        // Añade el listener (la acción de click)
        botonComponent.onClick.RemoveAllListeners();
        botonComponent.onClick.AddListener(OnClick);
    }

    // Esta función se ejecuta cuando el jugador hace click en este hexágono
    private void OnClick()
    {
        uiManager.OnAccionClicked(miAccion);
    }

    // El UIManager llama a esto para actualizar el estado visual
    public void ActualizarEstado(bool desbloqueado)
    {
        botonComponent.interactable = desbloqueado;
        imagenFondo.color = desbloqueado ? colorDesbloqueado : colorBloqueado;
        //icono.color = desbloqueado ? colorDesbloqueado : colorBloqueado;
    }

    public int GetAccionID()
    {
        return miAccion.idAccion;
    }
}