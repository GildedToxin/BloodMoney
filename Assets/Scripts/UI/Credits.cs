using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public void backButton()
    {
        GameManager.Instance.PlayUIButtonPress();
        SceneManager.UnloadSceneAsync("Credits");
    }
    public void Hover()
    {
        GameManager.Instance.ButtonHover();
    }
}
