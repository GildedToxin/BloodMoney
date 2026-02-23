using UnityEngine;

public class GiftShopBox : MonoBehaviour
{
    public bool hasShownScreen;
    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.currentDay == 0 && !hasShownScreen && FindAnyObjectByType<FirstDayManager>().currentScreen == 12)
        {
            var fdm = FindAnyObjectByType<FirstDayManager>();
            fdm.currentScreen++;
            fdm.isShowingScreen = true;
            fdm.tutorialScreens[fdm.currentScreen].SetActive(true);
            hasShownScreen = true;
            StartCoroutine(fdm.WaitForNextScreen());
        }
    }
}
