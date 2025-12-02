using UnityEngine;

public class ElevatorButtonScript : MonoBehaviour, IPlayerLookTarget
{
    //public bool isPressed = false;
    public int buttonFloor;
    public EvelatorTeleporter elevatorScript;
    //public Interact interactScript;

    private bool isLookedAt = false;

    private void Update()
    {
        if (isLookedAt && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
            //interactScript.noInteraction = true;
        } 
    }
    public void Interact()
    {
        elevatorScript.targetFloor = buttonFloor;
        Debug.Log("Button Pressed");
    }

    public void OnLookEnter()
    {
        FindAnyObjectByType<HUDManager>().CrossHairText.SetActive(true);
        FindAnyObjectByType<HUDManager>().UpdateCrossHairText("Press E to call elevator to floor " + buttonFloor);
        isLookedAt = true;
    }

    public void OnLookExit()
    {
        FindAnyObjectByType<HUDManager>().CrossHairText.SetActive(false);
        isLookedAt = false;
    }
}
