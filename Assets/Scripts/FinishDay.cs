using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishDay : MonoBehaviour, IPlayerLookTarget
{
 public bool isLookedAt = false;

    public void Update()
    {
        if (isLookedAt && Input.GetKeyDown(KeyCode.E) && GameManager.Instance.HasPlayerHitQuota())
        {
            if (GameManager.Instance.currentDay == 9)
            {
                GameManager.Instance.PlayEndSequence();
            }
            else
            {
                FindAnyObjectByType<HUDManager>().CrossHairText.transform.parent.parent.gameObject.SetActive(false);
                // EndVerticalSlice();
                GameManager.Instance.currentDay++;
                GameManager.Instance.moneyMadeToday = 0;
                SceneManager.LoadScene("Hotel");
            }
        }


    }

    public void OnLookEnter()
    {
        FindAnyObjectByType<HUDManager>().CrossHairText.transform.parent.parent.gameObject.SetActive(true);
        if (GameManager.Instance.HasPlayerHitQuota())
            FindAnyObjectByType<HUDManager>().UpdateCrossHairText("Press E to end your shift");
        else
            FindAnyObjectByType<HUDManager>().UpdateCrossHairText("You must complete your quota");

        isLookedAt = true;
    }
    public void OnLookExit()
    {
        FindAnyObjectByType<HUDManager>().CrossHairText.transform.parent.parent.gameObject.SetActive(false);
        isLookedAt = false;
    }

    public void EndVerticalSlice()
    {
        FindAnyObjectByType<EndGameCanvas>(FindObjectsInactive.Include).gameObject.SetActive(true);
        FindAnyObjectByType<EndGameCanvas>(FindObjectsInactive.Include).lostGame.SetActive(false);
        FindAnyObjectByType<EndGameCanvas>(FindObjectsInactive.Include).wonGame.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        FindAnyObjectByType<PlayerController>().enabled = false;
    }
}
