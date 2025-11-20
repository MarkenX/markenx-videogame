using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    [Header("Paneles Principales")]
    public GameObject panelConsumidor;
    public GameObject panelAtributos;
    public GameObject panelExploracion;
    public GameObject panelPublicidad;
    public GameObject panelPerfil;
    public GameObject panelNoticiaOverlay;

    [Header("Alertas")]
    public GameObject alertaPerfil;

    [Header("Elementos UI Estado")]
    public TextMeshProUGUI textoTurno;
    public TextMeshProUGUI textoPresupuesto;
    public TextMeshProUGUI textoNoticiaTopBar;
    public Image imagenPersonaje;
    public Sprite spriteNormal;
    public Sprite spriteFeliz;


    [Header("Presupuesto (Multiples sitios)")]
    public TextMeshProUGUI textoPresupuestoPrincipal; // Group_Consumidor
    public List<TextMeshProUGUI> textosPresupuestoPaneles; // Los textos dentro de los Button_Cash de cada panel

    [Header("Sliders con Porcentaje")]
    public Slider sliderAceptacion;
    public TextMeshProUGUI textoPorcentajeAceptacion;
    public Slider sliderPerfil;
    public TextMeshProUGUI textoPorcentajePerfil;

    [Header("Noticia Overlay")]
    public TextMeshProUGUI textoNoticiaTitulo;
    public TextMeshProUGUI textoNoticiaCuerpo;
    public Button botonCerrarNoticia;

    [Header("Contenedores Hexágonos")]
    public Transform contenedorAtributos;
    public Transform contenedorExploracion;
    public Transform contenedorPublicidad;
    
    [Header("Botones Categoría (Atributos)")]
    public Button botonCategoriaProduccion; 
    public Button botonCategoriaDiseno;
    public Button botonCategoriaPrecio;
    public Button botonCategoriaPlaza;

    [Header("Detalle Acción")]
    public GameObject panelDetalle;
    public TextMeshProUGUI detalleNombre;
    public TextMeshProUGUI detalleDescrip;
    public TextMeshProUGUI detallePrecio;
    public Button botonInvertir;

    [Header("Perfil UI")]
    public TextMeshProUGUI perfilNombre;
    public TextMeshProUGUI perfilEdad;
    public List<TextMeshProUGUI> textosFactores;

    [Header("Control Final")]
    public Button buttonEnviar;
    public TextMeshProUGUI textoBotonEnviar; // Para cambiar a "TERMINAR"

    [Header("Recursos")]
    public GameObject hexagonoPrefab;
    public string escenaMainMenu = "MainMenu";

    private GameSceneManager manager;
    private Accion accionSeleccionada;
    private int factoresDescubiertosPrevios = 0;

    void Start()
    {
        manager = GameSceneManager.Instance;
        if (manager == null) return;

        // --- CONEXIÓN SEGURA DE BOTONES DE ACCIÓN ---
        if (botonInvertir != null) 
        {
            botonInvertir.onClick.RemoveAllListeners(); // Limpia por si acaso
            botonInvertir.onClick.AddListener(OnInvertirClick);
        }
        else Debug.LogError("FALTA ASIGNAR: Boton Invertir en el Inspector");

        if (buttonEnviar != null) 
        {
            buttonEnviar.onClick.RemoveAllListeners();
            buttonEnviar.onClick.AddListener(OnEnviarTurnoClick);
        }
        else Debug.LogError("FALTA ASIGNAR: Button Enviar en el Inspector");

        // Guardar estado inicial de factores para saber si hay nuevos
        factoresDescubiertosPrevios = manager.GetSubfactoresDescubiertos().Count;

        // 1. Configurar Botones de Categoría
        if (botonCategoriaProduccion != null) botonCategoriaProduccion.onClick.AddListener(() => PoblarPanelAcciones("ATRIBUTOS_PRODUCCION"));
        if (botonCategoriaDiseno != null) botonCategoriaDiseno.onClick.AddListener(() => PoblarPanelAcciones("ATRIBUTOS_DISENO"));
        if (botonCategoriaPrecio != null) botonCategoriaPrecio.onClick.AddListener(() => PoblarPanelAcciones("ATRIBUTOS_PRECIO"));
        if (botonCategoriaPlaza != null) botonCategoriaPlaza.onClick.AddListener(() => PoblarPanelAcciones("ATRIBUTOS_PLAZA"));

        if(botonCerrarNoticia) botonCerrarNoticia.onClick.AddListener(CerrarNoticiaOverlay);
        
        // 2. Estado inicial de paneles
        OcultarTodosLosPaneles();
        if (panelConsumidor != null) panelConsumidor.SetActive(true);
        if (panelDetalle != null) panelDetalle.SetActive(false);
        if (panelNoticiaOverlay != null) panelNoticiaOverlay.SetActive(false);
        if (alertaPerfil != null) alertaPerfil.SetActive(false); // Alerta apagada al inicio
        
        // 3. Generar Hexágonos iniciales
        // (Se generan una vez en sus contenedores y luego solo se actualiza su estado visual)
        GenerarHexagonos("ATRIBUTOS_PRODUCCION", contenedorAtributos);
        GenerarHexagonos("ATRIBUTOS_DISENO", contenedorAtributos); // Se añaden al mismo
        GenerarHexagonos("ATRIBUTOS_PRECIO", contenedorAtributos);
        GenerarHexagonos("ATRIBUTOS_PLAZA", contenedorAtributos);
        GenerarHexagonos("EXPLORACION", contenedorExploracion);
        GenerarHexagonos("PUBLICIDAD", contenedorPublicidad);

        // 4. Inicializar UI con datos
        ActualizarEstadoUI();
        PoblarPerfil();

        // 5. Forzar visualización inicial de Producción
        PoblarPanelAcciones("ATRIBUTOS_PRODUCCION");
    }

    // LÓGICA DE GENERACIÓN

    void GenerarHexagonos(string categoria, Transform contenedor)
    {
        if (contenedor == null) return;

        var acciones = manager.GetPartidaData().accionesDisponibles
                       .Where(a => a.categoria == categoria).ToList();

        foreach (var acc in acciones)
        {
            GameObject hex = Instantiate(hexagonoPrefab, contenedor);
            AccionButton btn = hex.GetComponent<AccionButton>();
            if(btn != null) btn.Inicializar(acc, this);
            
            // Se crean desactivados. 'PoblarPanelAcciones' los activará.
            // Solo para atributos. Para Exploración y Publicidad los activamos directo.
            if (contenedor == contenedorAtributos)
                hex.SetActive(false);
            else
                hex.SetActive(true);
        }
    }
    
    // Esta función filtra qué hexágonos se ven en el panel compartido (Atributos)
    void PoblarPanelAcciones(string categoriaMostrar)
    {
        if (contenedorAtributos == null) return;

        foreach (Transform child in contenedorAtributos)
        {
            AccionButton btn = child.GetComponent<AccionButton>();
            if (btn != null)
            {
                // 1. Decidir si se muestra este hexágono (si es de la categoría correcta)
                bool coincide = (btn.GetAccionCategoria() == categoriaMostrar);
                child.gameObject.SetActive(coincide);

                // Si se lo va a mostrar, se debe actualizar su color.
                if (coincide)
                {
                    bool desbloqueada = manager.IsAccionDesbloqueada(btn.GetAccionID());
                    bool comprada = manager.IsAccionComprada(btn.GetAccionID());
                    btn.ActualizarVisual(desbloqueada, comprada);
                }
            }
        }
    }

    // ACTUALIZACIÓN DE UI

    public void ActualizarEstadoUI()
    {
        // 1. Textos Generales
        if (textoTurno) textoTurno.text = "TURNO " + manager.GetTurnoActual();
        
        string presupuestoStr = "$" + manager.GetPresupuestoActual();
        if (textoPresupuestoPrincipal) textoPresupuestoPrincipal.text = presupuestoStr;
        // Actualizar TODOS los botones de Cash en sub-paneles
        foreach(var texto in textosPresupuestoPaneles) {
            if(texto != null) texto.text = presupuestoStr;
        }

        // 2. Sliders y Porcentajes
        float aceptacion = manager.GetAceptacionActual();
        if (sliderAceptacion) sliderAceptacion.value = aceptacion / 100f;
        if (textoPorcentajeAceptacion) textoPorcentajeAceptacion.text = aceptacion.ToString("F0") + "%";

        float nivelPerfil = manager.GetNivelPerfil();
        if (sliderPerfil) sliderPerfil.value = nivelPerfil;
        if (textoPorcentajePerfil) textoPorcentajePerfil.text = (nivelPerfil * 100).ToString("F0") + "%";

        // 3. Estado del Personaje y Botón Final
        if (aceptacion >= 80)
        {
            if (imagenPersonaje) imagenPersonaje.sprite = spriteFeliz;
            if (textoBotonEnviar) textoBotonEnviar.text = "TERMINAR";
        }
        else
        {
            if (imagenPersonaje) imagenPersonaje.sprite = spriteNormal;
            if (textoBotonEnviar) textoBotonEnviar.text = "ENVIAR";
        }

        // 4. Alerta de Perfil (Lógica)
        int factoresAhora = manager.GetSubfactoresDescubiertos().Count;
        if (factoresAhora > factoresDescubiertosPrevios)
        {
            if (alertaPerfil) alertaPerfil.SetActive(true); // ENCENDER ALERTA
            factoresDescubiertosPrevios = factoresAhora;
        }

        // 5. Noticia
        string tituloNoticia = manager.GetNoticiaTitulo();
        if (!string.IsNullOrEmpty(tituloNoticia))
        {
            if (textoNoticiaTopBar) textoNoticiaTopBar.text = tituloNoticia;
            if (panelNoticiaOverlay && !panelNoticiaOverlay.activeSelf && manager.GetTurnoActual() == 3) {
                 panelNoticiaOverlay.SetActive(true);
                 if (textoNoticiaTitulo) textoNoticiaTitulo.text = tituloNoticia;
                 if (textoNoticiaCuerpo) textoNoticiaCuerpo.text = manager.GetNoticiaDetalle();
             }
        }
        else { if (textoNoticiaTopBar) textoNoticiaTopBar.text = "SIN NOTICIAS"; }
        
        ActualizarEstadoHexagonos();
        PoblarPerfil();
    }

    void ActualizarEstadoHexagonos()
    {
        var botones = FindObjectsByType<AccionButton>(FindObjectsSortMode.None);
        foreach(var btn in botones)
        {
            bool desbloqueada = manager.IsAccionDesbloqueada(btn.GetAccionID());
            bool comprada = manager.IsAccionComprada(btn.GetAccionID());
            btn.ActualizarVisual(desbloqueada, comprada); // Actualizado para manejar comprados
        }
    }

    void PoblarPerfil()
    {
        var data = manager.GetPartidaData();
        if (perfilNombre != null) perfilNombre.text = data.nombreConsumidor;
        if (perfilEdad != null) perfilEdad.text = data.edadConsumidor + " AÑOS";

        // Mapeo para Demo
        SetTextoFactor(0, 100, "SUBCULTURA: ECOLÓGICO");
        SetTextoFactor(1, 200, "OCUPACIÓN: PILOTO");
        SetTextoFactor(2, 300, "ESTILO: MINIMALISTA");
        SetTextoFactor(3, 400, "MOTIVACIÓN: AHORRO");
    }

    void SetTextoFactor(int indexUI, int idFactor, string textoReal)
    {
        if (indexUI < textosFactores.Count)
        {
            if (manager.IsFactorDescubierto(idFactor))
                textosFactores[indexUI].text = textoReal;
            else
                textosFactores[indexUI].text = "??";
        }
    }

    // EVENTOS DE CLICK (UI)

    public void OnHexagonoClick(Accion accion)
    {
        // SI YA SE COMPRÓ, NO HACER NADA (O SOLO MOSTRAR DETALLE SIN BOTÓN)
        bool yaComprada = manager.IsAccionComprada(accion.idAccion);

        accionSeleccionada = accion;
        if (panelDetalle != null)
        {
            panelDetalle.SetActive(true);
            if (detalleNombre != null) detalleNombre.text = accion.nombreAccion;
            if (detalleDescrip != null) detalleDescrip.text = accion.descripcion;
            if (detallePrecio != null) detallePrecio.text = "$" + accion.costo;
            
            // Validar compra
            bool puedeComprar = manager.GetPresupuestoActual() >= accion.costo && manager.IsAccionDesbloqueada(accion.idAccion);
            // BLOQUEAR SI YA SE COMPRÓ
            if (botonInvertir) 
            {
                botonInvertir.interactable = puedeComprar && !yaComprada;
                // Opcional: Cambiar texto a "COMPRADO"
                var textoBtn = botonInvertir.GetComponentInChildren<TextMeshProUGUI>();
                if(textoBtn) textoBtn.text = yaComprada ? "COMPRADO" : "INVERTIR";
            }
        }
    }

    public void OnInvertirClick()
    {
        if(accionSeleccionada != null)
        {
            if(manager.ComprarAccion(accionSeleccionada.idAccion))
            {
                if (panelDetalle != null) panelDetalle.SetActive(false);
                ActualizarEstadoUI();
            }
        }
    }

    public void OnEnviarTurnoClick()
    {
        // LOGICA: SI ES > 80%, ESTE BOTÓN TERMINA EL JUEGO
        if (manager.GetAceptacionActual() >= 80)
        {
            manager.EjecutarFinDeJuego("GANASTE");
        }
        else
        {
            manager.TerminarTurno();
            ActualizarEstadoUI();
        }
    }

    public void CerrarNoticiaOverlay()
    {
        if(panelNoticiaOverlay != null) panelNoticiaOverlay.SetActive(false);
    }

    public void ShowPanelPerfil()
    {
        OcultarTodosLosPaneles();
        if (panelPerfil != null) AnimatePanelOpening(panelPerfil);
        
        // APAGAR ALERTA AL ENTRAR
        if (alertaPerfil != null) alertaPerfil.SetActive(false);
    }

    // FUNCIONES DE NAVEGACIÓN Y ANIMACIÓN

    private void OcultarTodosLosPaneles()
    {
        if (panelConsumidor != null) panelConsumidor.SetActive(false);
        if (panelAtributos != null) panelAtributos.SetActive(false);
        if (panelExploracion != null) panelExploracion.SetActive(false);
        if (panelPublicidad != null) panelPublicidad.SetActive(false);
        if (panelPerfil != null) panelPerfil.SetActive(false);
        
        // Ocultar detalle al cambiar de pestaña
        if (panelDetalle != null) panelDetalle.SetActive(false);
    }

    // Animación simple
    public void AnimatePanelOpening(GameObject panel)
    {
        panel.SetActive(true);
        panel.transform.localScale = Vector3.zero;
        StartCoroutine(PopUpAnimation(panel.transform));
    }

    IEnumerator PopUpAnimation(Transform target)
    {
        float timer = 0;
        float duration = 0.2f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = timer / duration;
            float scale = Mathf.Sin(progress * Mathf.PI * 0.5f); 
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
        // Asegurar que se vean los hexágonos correctos
        PoblarPanelAcciones("ATRIBUTOS_PRODUCCION");
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
    
    // Botones TopBar
    public void BotonPausa() { Debug.Log("Pausa"); }
    public void BotonInicio() { ShowPanelConsumidor(); }
    public void BotonIrAMenu() { SceneManager.LoadScene(escenaMainMenu); }
}


/* =================================================================================
   CÓDIGO ANTIGUO (COMENTADO)
   =================================================================================
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
================================================================================= */