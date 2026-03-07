using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DaySelectManager : MonoBehaviour
{
    [SerializeField] CanvasGroup mainMenu;
    [SerializeField] CanvasGroup dayMenu;

    public UnityEngine.UI.Button[] days = new UnityEngine.UI.Button[10];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int i = 0;
        foreach(UnityEngine.UI.Button button in days)
        {
            days[i].interactable = false;
            days[i].transform.GetChild(1    ).gameObject.SetActive(false);
            if (i <= GameManager.Instance.highestReachedDay)
            {
                days[i].interactable = true;
                if(i != GameManager.Instance.highestReachedDay)
                {
                    days[i].transform.GetChild(1).gameObject.SetActive(true);
                }
            }



            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartGameOnDay(int day)
    {

        GameManager.Instance.currentDay = day;
        SceneManager.LoadSceneAsync("Hotel");
    }

    public void ReturnToMenu()
    {
        dayMenu.alpha = 0;
        dayMenu.interactable = false;
        dayMenu.blocksRaycasts = false;
        mainMenu.alpha = 1;
        mainMenu.interactable = true;
        mainMenu.blocksRaycasts = true;

        Debug.Log("working");
    }

}
