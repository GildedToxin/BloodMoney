using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static Unity.Collections.AllocatorManager;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject controlsMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject AreUSure;

    public AudioPool audiopool;
    public AudioClip buttonHover;
    public AudioClip Click;
    private void Start()
    {
        GameManager.Instance.pauseMenu = this;   
        gameObject.SetActive(false);

    }

    public void Resume()
    {
        audiopool.PlayClip2D(Click);
        Time.timeScale = 1f;
        GameManager.Instance.pauseMenu.gameObject.SetActive(false);
            GameManager.Instance.isPaused = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
            return;

    }

    public void ControlsMenu()
    {
        SceneManager.LoadScene("Control_Info", LoadSceneMode.Additive);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        GameManager.Instance.pauseMenu.gameObject.SetActive(true);
        GameManager.Instance.isPaused = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        return;
    }

    public void AreYouSure()
    {
        AreUSure.SetActive(true);
    }
    public void NotSure()
    {
        AreUSure.SetActive(false);
    }
    public void MainMenu()
    {
        Time.timeScale = 1f;
        GameManager.Instance.pauseMenu.gameObject.SetActive(false);
        GameManager.Instance.isPaused = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void SettingsMenu()
    {
        settingsMenu.SetActive(true);
        this.gameObject.SetActive(false );
        audiopool.PlayClip2D(Click);
    }

    public void ButtonHover()
    {
        audiopool.PlayClip2D(buttonHover);
    }
}
