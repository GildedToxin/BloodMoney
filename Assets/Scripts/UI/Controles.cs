using UnityEngine;
using UnityEngine.SceneManagement;
public class Controles : MonoBehaviour
{
    public void backButton()
    {
        GameManager.Instance.PlayUIButtonPress();
        SceneManager.UnloadSceneAsync("Control_Info");
    }
    public void Hover()
    {
        GameManager.Instance.ButtonHover();
    }
}
