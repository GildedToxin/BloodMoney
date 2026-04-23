using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OutroSequence : MonoBehaviour
{

    public GameObject logo;
    [ContextMenu("StartOutro")]
 public void StartOutro()
    {
        StartCoroutine(Outro());
    }

    public IEnumerator Outro()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(true);
            yield return new WaitForSeconds(3f);
            child.gameObject.SetActive(false);
        }
        logo.SetActive(true);  
        SceneManager.LoadScene("MainMenu");
    }
}
