using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishDay : MonoBehaviour, IPlayerLookTarget
{
 public bool isLookedAt = false;

    public void Update()
    {
        if (isLookedAt && Input.GetKeyDown(KeyCode.E) && GameManager.Instance.HasPlayerHitQuota())
        {
            FindAnyObjectByType<HUDManager>().CrossHairText.SetActive(false);
            // EndVerticalSlice();
            GameManager.Instance.currentDay++;
           SceneManager.LoadScene("Hotel");
        }
    }

    public void OnLookEnter()
    {
        FindAnyObjectByType<HUDManager>().CrossHairText.SetActive(true);
        if (GameManager.Instance.HasPlayerHitQuota())
            FindAnyObjectByType<HUDManager>().UpdateCrossHairText("Press E to leave the hotel and end your shift");
        else
            FindAnyObjectByType<HUDManager>().UpdateCrossHairText("You must complete your quota before ending your shift");

        isLookedAt = true;
    }
    public void OnLookExit()
    {
        FindAnyObjectByType<HUDManager>().CrossHairText.SetActive(false);
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
