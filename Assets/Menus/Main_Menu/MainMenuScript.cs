using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{

    [SerializeField] CanvasGroup settingsMenu;
    [SerializeField] CanvasGroup mainMenu;
    [SerializeField] CanvasGroup dayMenu;

    public Button playGame;



    public void Start()
    {
        if (GameManager.Instance.highestReachedDay != 0)
        {
            playGame.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Contiune");
        }
    }


    public void PlayGame()
    {
        GameManager.Instance.currentDay = GameManager.Instance.highestReachedDay;
        SceneManager.LoadSceneAsync("Hotel");
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
    public void SelectADay()
    {
            dayMenu.alpha = 1;
            dayMenu.interactable = true;
            dayMenu.blocksRaycasts = true;
            mainMenu.alpha = 0;
            mainMenu.interactable = false;
            mainMenu.blocksRaycasts = false;
    }
    public void CreditsScene()
    {
        SceneManager.LoadScene("Credits", LoadSceneMode.Additive);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
