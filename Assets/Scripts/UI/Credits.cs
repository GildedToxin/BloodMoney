using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public void backButton()
    {
        SceneManager.UnloadSceneAsync("Credits");
    }
}
