using UnityEngine;
using UnityEngine.SceneManagement;
public class Controles : MonoBehaviour
{
    public void backButton()
    {
        SceneManager.UnloadSceneAsync("Control_Info");
    }
}
