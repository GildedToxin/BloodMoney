using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void NewGame()
    {
        SceneManager.LoadSceneAsync("Lobby");
    }

    public void ContinueGame()
    {
        SceneManager.LoadSceneAsync("ProgrammingTestScene 1");
    }

    public void StatsMenu()
    {
        Debug.Log("Stats");
    }

    public void ControlsMenu()
    {
        Debug.Log("Controls");
    }

    public void CreditsScene()
    {
        Debug.Log("Credits");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
