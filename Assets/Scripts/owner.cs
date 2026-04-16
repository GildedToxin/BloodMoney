using UnityEngine;

public class owner : MonoBehaviour, IPlayerLookTarget
{
  
    public bool isLookedAt = false;

    public bool hasShownScreen = false;


    private void Update()
    {
        if(!GameManager.Instance.doesPlayerHaveKey && isLookedAt && Input.GetKeyDown(KeyCode.E))
        {
            /*if (GameManager.Instance.currentDay == 0 && !hasShownScreen && FindAnyObjectByType<FirstDayManager>().currentScreen == 3)
            {
                var fdm = FindAnyObjectByType<FirstDayManager>();
                fdm.currentScreen++;
                fdm.isShowingScreen = true;
                fdm.tutorialScreens[fdm.currentScreen].SetActive(true);
                hasShownScreen = true;
                StartCoroutine(fdm.WaitForNextScreen());
            }*/



            FindAnyObjectByType<HUDManager>().CrossHairText.transform.parent.parent.gameObject.SetActive(false);
            GameManager.Instance.GivePlayerKey();
            GameManager.Instance.doesPlayerHaveKey = true;
            GameManager.SetLayerRecursively(transform.GetChild(0).gameObject, LayerMask.NameToLayer("Default"));
         
        }
    }
    public void OnLookEnter()
    {
        FindAnyObjectByType<HUDManager>().CrossHairText.transform.parent.parent.gameObject.SetActive(true);
        if (!GameManager.Instance.doesPlayerHaveKey)
            FindAnyObjectByType<HUDManager>().UpdateCrossHairText("Talk to Randy");
        else
            FindAnyObjectByType<HUDManager>().UpdateCrossHairText("You already have a Key");

        isLookedAt = true;

    }
    public void OnLookExit()
    {
        FindAnyObjectByType<HUDManager>().CrossHairText.transform.parent.parent.gameObject.SetActive(false);
        isLookedAt = false;
    }
}
