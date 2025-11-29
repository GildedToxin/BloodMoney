using UnityEngine;

public class FinishDay : MonoBehaviour, IPlayerLookTarget
{
 public bool isLookedAt = false;

    public void Update()
    {
        if (isLookedAt && Input.GetKeyDown(KeyCode.E))
        {
            FindAnyObjectByType<HUDManager>().CrossHairText.SetActive(false);
            if (GameManager.Instance.HasPlayerHitQuota())
            {
                EndVerticalSlice();
            }
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
        //Load UI Stuff
    }
}
