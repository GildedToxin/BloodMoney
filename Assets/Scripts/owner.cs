using UnityEngine;

public class owner : MonoBehaviour, IPlayerLookTarget
{
  
    public bool isLookedAt = false;

    public bool hasShownScreen = false;


    private void Update()
    {
        if(!GameManager.Instance.doesPlayerHaveKey && isLookedAt && Input.GetKeyDown(KeyCode.E) && !hasShownScreen)
        {
            if (GameManager.Instance.currentDay == 0 && !hasShownScreen && FindAnyObjectByType<FirstDayManager>().currentScreen == 3)
            {
                var fdm = FindAnyObjectByType<FirstDayManager>();
                fdm.currentScreen++;
                fdm.isShowingScreen = true;
                fdm.tutorialScreens[fdm.currentScreen].SetActive(true);
                hasShownScreen = true;
                StartCoroutine(fdm.WaitForNextScreen());
            }



            FindAnyObjectByType<HUDManager>().CrossHairText.SetActive(false);
            GameManager.Instance.GivePlayerKey();
            GameManager.Instance.doesPlayerHaveKey = true;
            transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }
    public void OnLookEnter()
    {
        FindAnyObjectByType<HUDManager>().CrossHairText.SetActive(true);
        if(!GameManager.Instance.doesPlayerHaveKey)
            FindAnyObjectByType<HUDManager>().UpdateCrossHairText("Press E to collect room Key");
        else
            FindAnyObjectByType<HUDManager>().UpdateCrossHairText("You already have the room Key");

        isLookedAt = true;

    }
    public void OnLookExit()
    {
        FindAnyObjectByType<HUDManager>().CrossHairText.SetActive(false);
        isLookedAt = false;
    }
}
