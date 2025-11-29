using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameCanvas : MonoBehaviour
{
    public GameObject lostGame;
    public GameObject wonGame;

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
