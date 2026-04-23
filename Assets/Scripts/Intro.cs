using System.Collections;
using UnityEditor.Rendering.HighDefinition;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{

    public GameObject logo;
    public bool intro;


    public void Start()
    {
        if(GameManager.Instance.currentDay == 0)
        {
            StartIntro();
        }
    }
    [ContextMenu("StartOutro")]
    public void StartIntro()
    {
        AudioListener.volume = 0f;
        FindAnyObjectByType<HUDManager>().gameObject.SetActive(false);
        intro = true;
        StartCoroutine(IntroSequence());
    }

    public IEnumerator IntroSequence()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            child.gameObject.SetActive(false);
        }
        intro = false;
        //logo.SetActive(true);
        //SceneManager.LoadScene("MainMenu");
        FindAnyObjectByType<HUDManager>(FindObjectsInactive.Include).gameObject.SetActive(true);
        AudioListener.volume = 1f;
    }
}
