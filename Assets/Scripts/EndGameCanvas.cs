using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameCanvas : MonoBehaviour
{
    public GameObject terminated;
    public GameObject sucess;

    public GameObject retry;
    public GameObject finish;
    public GameObject contiune;


    public GameObject dayText;
    public GameObject quotaText;
    public GameObject moneyMadeText;
    public GameObject noteText;


    public GameObject quotaBar;
    public void EndDay(bool hitQuota)
    {
        if (hitQuota)
        {
            terminated.SetActive(false);
            retry.SetActive(false);

            sucess.SetActive(true);

            if (GameManager.Instance.currentDay == 9)
            {
                finish.SetActive(true);
                contiune.SetActive(false);
            }
            else
            {
                contiune.SetActive(true);
                finish.SetActive(false);
            }
        }
        else
        {
            
            terminated.SetActive(true);
            retry.SetActive(true);

            sucess.SetActive(false);
            contiune.SetActive(false);
            finish.SetActive(false);
        }



        dayText.GetComponent<TMPro.TextMeshProUGUI>().text = "" + (GameManager.Instance.currentDay + 1);
        quotaText.GetComponent<TMPro.TextMeshProUGUI>().text = "" + GameManager.Instance.quota[GameManager.Instance.currentDay];
        moneyMadeText.GetComponent<TMPro.TextMeshProUGUI>().text = "" + GameManager.Instance.moneyMadeToday;


        quotaBar.GetComponent<Image>().fillAmount = Mathf.Clamp((float)GameManager.Instance.moneyMadeToday / GameManager.Instance.quota[GameManager.Instance.currentDay], .15f, 1);

      

        if (hitQuota && GameManager.Instance.currentDay != 9)
            GameManager.Instance.currentDay += 1;
    }
    public void SaveAndQuit()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void Retry()
    {

        GameManager.Instance.LoadSceneAndAssign();
    }
    public void Contiune()
    {

        GameManager.Instance.LoadSceneAndAssign();
    }
    public void Finish()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
