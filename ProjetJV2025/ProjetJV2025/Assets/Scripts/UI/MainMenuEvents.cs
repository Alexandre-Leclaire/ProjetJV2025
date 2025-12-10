using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuEvents : MonoBehaviour
{
    public void OnQuitClick()
    {
        Application.Quit();
    }

    public void OnPlay()
    {
        SceneManager.LoadScene("Game Scene");
    }
}
