using UnityEngine;

public class UserInterfaceController : MonoBehaviour
{
    private enum UIState { NONE, MENU, PAUSE }
    private UIState uiState = UIState.NONE;

    void Start()
    {

    }

    void Update()
    {

    }

    public void OnPlayPressed()
    {
        print("Play button pressed");
    }

    public void OnSettingsPressed()
    {
        print("Settings pressed");
    }

    public void OnQuitPressed()
    {
        print("Quit button pressed");
    }
}
