using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{

    [SerializeField] CanvasGroup settingsMenu;
    [SerializeField] CanvasGroup mainMenu;
    public void NewGame()
    {
        SceneManager.LoadSceneAsync("Hotel");
    }

    public void ContinueGame()
    {
        SceneManager.LoadSceneAsync("ProgrammingTestScene 1");
    }

    public void StatsMenu()
    {
        Debug.Log("Stats");
    }

    public void SettingsMenu()
    {
        settingsMenu.alpha = 1;
        settingsMenu.interactable = true;
        settingsMenu.blocksRaycasts = true;
        mainMenu.alpha = 0;
        mainMenu.interactable = false;
        mainMenu.blocksRaycasts = false;

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
