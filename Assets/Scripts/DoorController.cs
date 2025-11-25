using UnityEngine;

public class DoorController : MonoBehaviour, IPlayerLookTarget
{
    public bool canBeOpened = false;
    public bool isOpened = false;
    public string RoomNumber;
    public GameObject pivot;
    public bool isLookedAt;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isLookedAt && GameManager.Instance.CurrentDoor != null && GameManager.Instance.CurrentDoor == this && !isOpened && GameManager.Instance.doesPlayerHaveKey)
        {
            FindAnyObjectByType<HUDManager>().CrossHairText.SetActive(false);


            GameManager.Instance.CurrentDoor.gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            GameManager.Instance.CurrentDoor.gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            GameManager.Instance.CurrentDoor.gameObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
            GameManager.Instance.CurrentDoor.transform.GetChild(0).GetChild(1).gameObject.layer = LayerMask.NameToLayer("Default");

            isOpened = true;
            GameManager.Instance.doesPlayerHaveKey = false;
            FindAnyObjectByType<HUDManager>().UpdateRoomNumber("");

        }
    }

    public void OnLookEnter()
    {
        if (GameManager.Instance.CurrentDoor != null && GameManager.Instance.CurrentDoor == this && !isOpened && GameManager.Instance.doesPlayerHaveKey)
        {
            if (!isOpened)
            {
                FindAnyObjectByType<HUDManager>().CrossHairText.SetActive(true);
                if (GameManager.Instance.CurrentDoor == this)
                    FindAnyObjectByType<HUDManager>().UpdateCrossHairText("Press E to open the door");
                else
                    FindAnyObjectByType<HUDManager>().UpdateCrossHairText("Your key doesn't fit in this door");
            }
            isLookedAt = true;
        }
    }
    public void OnLookExit()
    {
        FindAnyObjectByType<HUDManager>().CrossHairText.SetActive(false);
        isLookedAt = false;
    }
}
