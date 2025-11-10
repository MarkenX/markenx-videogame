using UnityEngine;
using UnityEngine.SceneManagement; 

public class GameUIManager : MonoBehaviour
{
    public GameObject contextPanel;
    public GameObject decisionPanel;
    public GameObject resultPanel;

    void Start()
    {
        if (resultPanel != null)
        {
            resultPanel.SetActive(false);
        }
    }

    public void ShowResultPanel()
    {
        if (decisionPanel != null)
        {
            decisionPanel.SetActive(false);
        }
        
        if (contextPanel != null)
        {
            contextPanel.SetActive(false);
        }

        if (resultPanel != null)
        {
            resultPanel.SetActive(true);
        }
    }

    public void RestartGame()
    {
        // Se llama explícitamente al SceneManager DE UNITY para evitar confusión
        string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentSceneName);
    }
}