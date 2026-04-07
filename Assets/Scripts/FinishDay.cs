using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishDay : MonoBehaviour, IPlayerLookTarget
{
 public bool isLookedAt = false;

    public void Update()
    {
        if (isLookedAt && Input.GetKeyDown(KeyCode.E) && GameManager.Instance.HasPlayerHitQuota())
        {
            FindAnyObjectByType<EndGameCanvas>(FindObjectsInactive.Include).gameObject.SetActive(true);
            FindAnyObjectByType<EndGameCanvas>().EndDay(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            FindAnyObjectByType<PlayerController>().enabled = false;
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
        FindAnyObjectByType<EndGameCanvas>().EndDay(true);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        FindAnyObjectByType<PlayerController>().enabled = false;
    }
}
