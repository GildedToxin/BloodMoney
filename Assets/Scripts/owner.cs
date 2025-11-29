using UnityEngine;

public class owner : MonoBehaviour, IPlayerLookTarget
{
  
    public bool isLookedAt = false;


    private void Update()
    {
        if(!GameManager.Instance.doesPlayerHaveKey && isLookedAt && Input.GetKeyDown(KeyCode.E))
        {
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
