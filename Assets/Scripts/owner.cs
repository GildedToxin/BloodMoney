using UnityEngine;

public class owner : MonoBehaviour, IPlayerLookTarget
{
    public bool doesPlayerHaveKey = false;
    public bool isLookedAt = false;


    private void Update()
    {
        if(!doesPlayerHaveKey && isLookedAt && Input.GetKeyDown(KeyCode.E))
        {
            GameManager.Instance.GivePlayerKey();
            doesPlayerHaveKey = true;
        }
    }
    public void OnLookEnter()
    {
        FindAnyObjectByType<HUDManager>().CrossHairText.SetActive(true);
        if(!doesPlayerHaveKey)
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
