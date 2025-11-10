using UnityEngine;
using UnityEngine.SceneManagement; 

public class GameSceneManager : MonoBehaviour
{
    // Carga una escena por su nombre
    public void LoadScene(string sceneName)
    {
        // Ahora se llama explícitamente al SceneManager DE UNITY
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    // Cierra la aplicación
    public void QuitGame()
    {
        Debug.Log("SALIENDO DEL JUEGO");
        Application.Quit();
    }
}