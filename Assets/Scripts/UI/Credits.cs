using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    public List<GameObject> credits = new List<GameObject>();
    public int listPosition = 0;
    public Button next;
    public Button prev;

    public void backButton()
    {
        GameManager.Instance.PlayUIButtonPress();
        SceneManager.UnloadSceneAsync("Credits");
    }
    public void Hover()
    {
        GameManager.Instance.ButtonHover();
    }

    public void nextButton()
    {
        listPosition++;
        loadCredits();
    }

    public void previousButton()
    {
        listPosition--;
        loadCredits();
    }

    public void loadCredits()
    {
        for (int i = 0; i < credits.Count; i++)
        {
            if (i == listPosition)
            {
                credits[i].SetActive(true);
            }
            else if (i != listPosition) 
            {
                credits[i].SetActive(false);
                    
            }
        }
    }
}
