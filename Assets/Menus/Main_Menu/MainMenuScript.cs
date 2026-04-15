using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{

    [SerializeField] GameObject settingsMenu;
    [SerializeField] CanvasGroup mainMenu;
    [SerializeField] CanvasGroup dayMenu;

    public Button playGame;

    private void Update()
    {
        if (!settingsMenu.activeSelf && mainMenu.alpha != 1)
        {
            settingsMenu.SetActive(true);
            mainMenu.alpha = 1;
            mainMenu.interactable = true;
            mainMenu.blocksRaycasts = true;
            settingsMenu.SetActive(false);
        }
    }

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
        settingsMenu.SetActive(true);
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
