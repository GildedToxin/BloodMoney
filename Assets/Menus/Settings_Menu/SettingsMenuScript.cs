using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuScript : MonoBehaviour
{
    [SerializeField] Toggle Window;
    [SerializeField] Toggle FullScreen;
    [SerializeField] CanvasGroup settingsMenu;
    [SerializeField] CanvasGroup mainMenu;
    public void WindowScreenSize (bool toggleValue)
    {

        if (Window.isOn)
        {
            Screen.fullScreen = false;//swaps screen to window
        }
    }

    public void FullScreneScreenSize(bool toggleValue)
    {

        if (FullScreen.isOn)
        {
            Screen.fullScreen = true; //swaps screen to full screen
        }

    }

    public void ReturnToMenu()
    {
        settingsMenu.alpha = 0;
        settingsMenu.interactable = false;  
        settingsMenu.blocksRaycasts = false;
        mainMenu.alpha = 1;
        mainMenu.interactable = true;
        mainMenu.blocksRaycasts = true;

        Debug.Log("working");
    }
}
