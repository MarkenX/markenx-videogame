using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    [Header("Componentes UI")]
    public CanvasGroup canvasGroup; // Para controlar la transparencia
    public Image imagenFondo;
    
    [Header("Configuración")]
    public float duracionFade = 0.5f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true; // Bloquear mientras carga
            StartCoroutine(FadeInInicial());
        }
    }
    IEnumerator FadeInInicial()
    {
        yield return StartCoroutine(Fade(0));
        canvasGroup.blocksRaycasts = false;
    }
    public void CargarEscena(string nombreEscena)
    {
        StartCoroutine(TransicionRutina(nombreEscena));
    }

    IEnumerator TransicionRutina(string escena)
    {
        // 1. Bloquear Raycasts para que no den clic a nada
        canvasGroup.blocksRaycasts = true;

        // 2. Fade Out (Pantalla se oscurece)
        yield return StartCoroutine(Fade(1));

        // 3. Cargar Escena
        AsyncOperation operacion = SceneManager.LoadSceneAsync(escena);
        while (!operacion.isDone)
        {
            yield return null;
        }

        // 4. Fade In (Pantalla se aclara)
        yield return StartCoroutine(Fade(0));

        // 5. Desbloquear
        canvasGroup.blocksRaycasts = false;
    }

    IEnumerator Fade(float alfaFinal)
    {
        float alfaInicial = canvasGroup.alpha;
        float tiempo = 0;

        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / duracionFade;
            t = t * t * (3f - 2f * t); 
            
            canvasGroup.alpha = Mathf.Lerp(alfaInicial, alfaFinal, t);
            yield return null;
        }
        canvasGroup.alpha = alfaFinal;
    }
}