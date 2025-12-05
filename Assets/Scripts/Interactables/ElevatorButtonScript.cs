using UnityEngine;

public class ElevatorButtonScript : MonoBehaviour, IPlayerLookTarget
{
    public int buttonFloor;
    public EvelatorTeleporter elevatorScript;

    public bool isLookedAt = false;

    private void Update()
    {
        if (isLookedAt && Input.GetKeyDown(KeyCode.E) && !EvelatorTeleporter.isMoving)
        {
            EvelatorTeleporter.isMoving = true;
            Interact();
            elevatorScript.StartCoroutine(elevatorScript.WaitOneSecond());
        } 
    }
    public void Interact()
    {
        elevatorScript.targetFloor = buttonFloor;
        elevatorScript.buttonPressed = true;
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
