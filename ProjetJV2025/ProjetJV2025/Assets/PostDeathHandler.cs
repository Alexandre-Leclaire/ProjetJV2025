using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PostDeathHandler : MonoBehaviour
{
    public Button restartButton;
    public Button returnToMenuButton;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       restartButton.onClick.AddListener(RestartGame); 
       returnToMenuButton.onClick.AddListener(ReturnToMenu); 
    }

    void RestartGame()
    {
        SceneManager.LoadScene(1);
    }

    void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
