using UnityEditor;
using UnityEngine;

public class DaySelectManager : MonoBehaviour
{

    public UIHover[] days = new UIHover[10];
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHover(UIHover hovered)
    {
        foreach(var day in days)
        {
            if (hovered.isLocked)
            {
                day.transform.GetChild(0).gameObject.SetActive(false);
                day.transform.GetChild(1).gameObject.SetActive(false);
                day.transform.GetChild(2).gameObject.SetActive(true);
                return;
            }
            else if (day == hovered)
            {
                day.transform.GetChild(0).gameObject.SetActive(false);
                day.transform.GetChild(1).gameObject.SetActive(true);
                day.transform.GetChild(2).gameObject.SetActive(false);
                return;
            }
            else
            {
                day.transform.GetChild(0).gameObject.SetActive(true);
                day.transform.GetChild(1).gameObject.SetActive(false);
                day.transform.GetChild(2).gameObject.SetActive(false);
                return;
            }
        }
    }
}
