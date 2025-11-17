using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [Header("Paneles Principales (Grupos)")]
    public GameObject panelConsumidor;    // Panel central con la barra de aceptación 
    public GameObject panelAtributos;
    public GameObject panelExploracion;  
    public GameObject panelPublicidad;    
    public GameObject panelPerfil;        

    [Header("UI de Estado (Panel Superior y Central)")]
    public TextMeshProUGUI textoTurno;
    public TextMeshProUGUI textoNoticia;
    public TextMeshProUGUI textoPresupuesto;
    public Slider sliderAceptacion;
    public Button buttonEnviar;

    [Header("UI Panel Atributos")]
    public Transform panelContenedorAtributos; // El 'Panel_HexGrid' de Atributos
    public GameObject panelDetalleAccion;
    public Button botonCategoriaProduccion; 
    public Button botonCategoriaDiseno;
    public Button botonCategoriaPrecio;
    public Button botonCategoriaPlaza;

    [Header("UI Panel Exploración")]
    public Transform panelContenedorExploracion; // El 'Panel_HexGrid' de Exploración

    [Header("UI Panel Publicidad")]
    public Transform panelContenedorPublicidad;  // El 'Panel_HexGrid' de Publicidad

    [Header("UI Panel Detalle Acción")]
    public TextMeshProUGUI textNombreAccion;
    public TextMeshProUGUI textDescripAccion;
    public TextMeshProUGUI textPrecioAccion;
    public Button botonInvertir;

    [Header("UI Panel Perfil")]
    public TextMeshProUGUI textNombrePerfil;
    public TextMeshProUGUI textEdadPerfil;
    public Slider sliderPerfil;
    public List<TextMeshProUGUI> camposTextoPerfil;

    [Header("Recursos (Prefabs)")]
    public GameObject prefabHexagono;

    [Header("Escenas")]
    public string escenaMainMenu = "MainMenu";

    private Accion accionSeleccionada;
    private GameSceneManager manager;


    void Start()
    {
        if (GameSceneManager.Instance == null)
        {
            Debug.LogError("ERROR: GameSceneManager no encontrado. Volviendo al Menú.");
            SceneManager.LoadScene(escenaMainMenu);
            return;
        }
        manager = GameSceneManager.Instance;

        // 2. Configurar Listeners de Botones de Categoría
        botonCategoriaProduccion.onClick.AddListener(() => PoblarPanelAcciones("ATRIBUTOS_PRODUCCION"));
        botonCategoriaDiseno.onClick.AddListener(() => PoblarPanelAcciones("ATRIBUTOS_DISENO"));
        botonCategoriaPrecio.onClick.AddListener(() => PoblarPanelAcciones("ATRIBUTOS_PRECIO"));
        botonCategoriaPlaza.onClick.AddListener(() => PoblarPanelAcciones("ATRIBUTOS_PLAZA"));

        // 3. Ocultar todos los paneles y mostrar el de consumidor
        ShowPanelConsumidor();
        panelDetalleAccion.SetActive(false);

        // 4. Poblar toda la UI con los datos cargados
        PoblarPanelPerfil();
        
        // 5. Poblar los paneles de acciones (HexGrids)
        PoblarPanelAcciones("ATRIBUTOS_PRODUCCION");
        PoblarPanelAcciones("EXPLORACION");
        PoblarPanelAcciones("PUBLICIDAD");

        // 6. Actualizar textos, sliders y botones
        ActualizarUIDeEstado();
    }

    // POBLADO DINÁMICO DE UI

    void PoblarPanelPerfil()
    {
        var data = manager.GetPartidaData();
        textNombrePerfil.text = "NOMBRE: " + data.nombreConsumidor;
        textEdadPerfil.text = "EDAD: " + data.edadConsumidor.ToString() + " AÑOS";

        var perfil = data.perfilConsumidor;
        var subfactoresDescubiertos = manager.GetSubfactoresDescubiertos();

        int i = 0;
        foreach (var factor in perfil)
        {
            if (i >= camposTextoPerfil.Count) break; // Evita error si faltan campos de texto

            if (subfactoresDescubiertos.Contains(factor.idSubfactor))
            {
                // Descubierto: Muestra el detalle real
                // (Se necesita buscar el 'detalle' en la lista de Subfactores que vendrá del backend)
                // *Por ahora, se usa un placeholder:*
                camposTextoPerfil[i].text = $"FACTOR {factor.idSubfactor} (DESCUBIERTO)"; 
            }
            else
            {
                camposTextoPerfil[i].text = "??";
            }
            i++;
        }
        
        // Limpia los campos de texto restantes
        for (; i < camposTextoPerfil.Count; i++)
        {
            camposTextoPerfil[i].text = "";
        }
    }

    void PoblarPanelAcciones(string categoria)
    {
        Transform panelContenedor;
        
        // 1. Determinar qué panel vamos a poblar
        if (categoria.StartsWith("ATRIBUTOS")) {
            panelContenedor = panelContenedorAtributos;
        } else if (categoria == "EXPLORACION") {
            panelContenedor = panelContenedorExploracion;
        } else if (categoria == "PUBLICIDAD") {
            panelContenedor = panelContenedorPublicidad;
        } else {
            return;
        }

        // 2. Limpiar hexágonos anteriores
        foreach (Transform child in panelContenedor)
        {
            Destroy(child.gameObject);
        }

        // 3. Filtrar acciones por la categoría seleccionada
        var acciones = manager.GetPartidaData().accionesDisponibles;
        List<Accion> accionesFiltradas = acciones.Where(a => a.categoria == categoria).ToList();

        // 4. Crear los nuevos hexágonos
        foreach (Accion accion in accionesFiltradas)
        {
            GameObject nuevoHex = Instantiate(prefabHexagono, panelContenedor);
            AccionButton btnScript = nuevoHex.GetComponent<AccionButton>();
            
            btnScript.Inicializar(accion, this); 
            
            // Actualiza el estado visual (bloqueado/desbloqueado)
            btnScript.ActualizarEstado(manager.EstaAccionDesbloqueada(accion.idAccion));
        }
    }

    // FUNCIONES DE BOTONES (UI)

    // Lo llama el script 'AccionButton.cs'
    public void OnAccionClicked(Accion accion)
    {
        accionSeleccionada = accion;
        panelDetalleAccion.SetActive(true);
        textNombreAccion.text = accion.nombreAccion;
        textDescripAccion.text = accion.descripcion;
        textPrecioAccion.text = accion.costo.ToString("C0");

        // Habilita el botón INVERTIR solo si está desbloqueada Y alcanza el presupuesto
        bool desbloqueada = manager.EstaAccionDesbloqueada(accion.idAccion);
        bool alcanzaPresupuesto = manager.GetPresupuestoActual() >= accion.costo;
        
        botonInvertir.interactable = desbloqueada && alcanzaPresupuesto;
    }

    // Llamado por el botón "Invertir"
    public void BotonInvertir()
    {
        if (accionSeleccionada == null) return;

        bool exito = manager.ComprarAccion(accionSeleccionada.idAccion);

        if (exito)
        {
            panelDetalleAccion.SetActive(false); // Oculta el panel post-compra
            ActualizarUIDeEstado(); // Actualiza presupuesto
            ActualizarArbolDeHabilidades();
        }
        else
        {
            Debug.LogWarning("¡Intento de compra fallido!");
        }
    }

    // Llamado por el botón "Enviar"
    public void EnviarTurno()
    {
        Debug.Log("Enviando turno al GameSceneManager...");
        buttonEnviar.interactable = false;

        // 1. Le dice al Manager que termine el turno
        manager.TerminarTurno();

        // 2. Actualiza toda la UI con los nuevos valores del Manager
        ActualizarUIDeEstado();

        // 3. Actualiza el panel de perfil por si se descubrió algo
        PoblarPanelPerfil();

        // 4. Reactiva el botón
        buttonEnviar.interactable = true;
    }

    // FUNCIONES DE ACTUALIZACIÓN DE UI

    // Actualiza todos los textos y sliders que cambian constantemente
    void ActualizarUIDeEstado()
    {
        textoPresupuesto.text = "$" + manager.GetPresupuestoActual().ToString();
        textoTurno.text = "TURNO " + manager.GetTurnoActual().ToString();
        textoNoticia.text = manager.GetNoticiaActual();
        
        sliderAceptacion.value = manager.GetAceptacionActual() / 100.0f;
        sliderPerfil.value = manager.GetNivelPerfil();
    }
    
    // Actualiza el estado visual (bloqueado/desbloqueado) de TODOS los hexágonos
    void ActualizarArbolDeHabilidades()
    {
        foreach (var panel in new Transform[] { panelContenedorAtributos, panelContenedorExploracion, panelContenedorPublicidad })
        {
            foreach (Transform hex in panel)
            {
                AccionButton btnScript = hex.GetComponent<AccionButton>();
                if (btnScript != null)
                {
                    btnScript.ActualizarEstado(manager.EstaAccionDesbloqueada(btnScript.GetAccionID()));
                }
            }
        }
    }


    // FUNCIONES DE NAVEGACIÓN (Sin cambios)

    // Función para ocultar todos los paneles intercambiables
    private void OcultarTodosLosPaneles()
    {
        if (panelConsumidor != null) panelConsumidor.SetActive(false);
        if (panelAtributos != null) panelAtributos.SetActive(false);
        if (panelExploracion != null) panelExploracion.SetActive(false);
        if (panelPublicidad != null) panelPublicidad.SetActive(false);
        if (panelPerfil != null) panelPerfil.SetActive(false);
    }

    // ANIMACIÓN EN PANELES
    public void AnimatePanelOpening(GameObject panel)
    {
        panel.SetActive(true);
        panel.transform.localScale = Vector3.zero;
        StartCoroutine(PopUpAnimation(panel.transform));
    }

    IEnumerator PopUpAnimation(Transform target)
    {
        float timer = 0;
        float duration = 0.3f;
        
        // Efecto "Overshoot"
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = timer / duration;
            
            // Curva de animación simple
            float scale = Mathf.Sin(progress * Mathf.PI * 0.5f) * 1.1f; 
            if (scale > 1f) scale = 1f;

            target.localScale = Vector3.one * scale;
            yield return null;
        }
        target.localScale = Vector3.one;
    }
    public void ShowPanelConsumidor()
    {
        OcultarTodosLosPaneles();
        if (panelConsumidor != null) AnimatePanelOpening(panelConsumidor);
    }

    public void ShowPanelAtributos()
    {
        OcultarTodosLosPaneles();
        if (panelAtributos != null) AnimatePanelOpening(panelAtributos);
    }

    public void ShowPanelExploracion()
    {
        OcultarTodosLosPaneles();
        if (panelExploracion != null) AnimatePanelOpening(panelExploracion);
    }

    public void ShowPanelPublicidad()
    {
        OcultarTodosLosPaneles();
        if (panelPublicidad != null) AnimatePanelOpening(panelPublicidad);
    }

    public void ShowPanelPerfil()
    {
        OcultarTodosLosPaneles();
        if (panelPerfil != null) AnimatePanelOpening(panelPerfil);
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

    public void BotonIrAMenu()
    {
        // Time.timeScale = 1; // (Asegurarse de quitar la pausa)
        SceneManager.LoadScene(escenaMainMenu);
    }
}