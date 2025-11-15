using System.Collections;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserInterfaceController : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenuCanvas;
    [SerializeField] private GameObject SettingsMenuCanvas;

    private bool isPaused = false;

    private enum UIState { NONE, MENU, PAUSE }
    private UIState uiState = UIState.NONE;

    private static UserInterfaceController UserInterface;

    void Awake()
    {
        // Prevention of duplicate UI gameObjects and makes UI persist between scenes
        if (UserInterface == null)
        {
            UserInterface = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        uiState = UIState.MENU; // Temporary line for testing menu in programming test scene
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && uiState != UIState.MENU)
        {
            if (isPaused) Unpause();
            else if (!isPaused) Pause();
        }
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0;
        PauseMenuCanvas.SetActive(true);
    }

    public void Unpause()
    {
        isPaused = false;
        Time.timeScale = 1.0f;
        PauseMenuCanvas.SetActive(false);
    }

    public void OnPlayPressed()
    {
        SceneManager.LoadScene("ProgrammingTestScene");
        uiState = UIState.PAUSE;
    }

    public void OnSettingsPressed()
    {
        switch (uiState)
        {
            case UIState.MENU:
                SettingsMenuCanvas.SetActive(true);
                break;
            case UIState.PAUSE:
                PauseMenuCanvas.SetActive(false);
                SettingsMenuCanvas.SetActive(true);
                break;

        }
    }

    public void OnQuitPressed()
    {
        print("Quit button pressed");
    }

    public void OnBackPressed()
    {
        switch (uiState)
        {
            case UIState.MENU:
                SettingsMenuCanvas.SetActive(false);
                break;
            case UIState.PAUSE:
                PauseMenuCanvas.SetActive(true);
                SettingsMenuCanvas.SetActive(false);
                break;
        }
    }

    public void OnQuitToMenuPressed()
    {
        SceneManager.LoadSceneAsync("MainMenu");
        PauseMenuCanvas.SetActive(false);
        uiState = UIState.MENU;
    }
}
