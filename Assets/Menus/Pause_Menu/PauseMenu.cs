using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject controlsMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject AreUSure;


    private void Start()
    {
        GameManager.Instance.pauseMenu= this;   
        gameObject.SetActive(false);

    }

    public void Resume()
    {
        Time.timeScale = 1f;
        GameManager.Instance.pauseMenu.gameObject.SetActive(false);
            GameManager.Instance.isPaused = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
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

}
