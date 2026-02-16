using UnityEngine;

public class ElevatorButtonScript : MonoBehaviour, IPlayerLookTarget
{
    public int buttonFloor; // Set to 0 in the inspector for the button to act as a elevator call button
    public EvelatorTeleporter elevatorScript;

    public bool isLookedAt = false;

    private void Update()
    {
        if (isLookedAt && Input.GetKeyDown(KeyCode.E) && !EvelatorTeleporter.isMoving)
        {
            if (buttonFloor != 0)
            {
                EvelatorTeleporter.isMoving = true;
                Interact();
                elevatorScript.StartCoroutine(elevatorScript.WaitOneSecond());
            }
            else
            {
                Interact();
            }
        } 
    }
    public void Interact()
    {
        if (buttonFloor != 0)
        {
            elevatorScript.targetFloor = buttonFloor;
            elevatorScript.buttonPressed = true;
            Debug.Log("Button Pressed");
        }
        else
        {
            foreach (GameObject elevator in elevatorScript.listOfEvelatorTeleporters)
            {
                if (elevator.GetComponent<EvelatorTeleporter>().doorsOpen)
                {
                    if (elevator.GetComponent<EvelatorTeleporter>().currentFloor == elevatorScript.currentFloor)
                        return;
                    else
                        elevator.GetComponent<EvelatorTeleporter>().doorsOpen = false; // Close the doors if they are open
                }
            }
            elevatorScript.doorsOpen = !elevatorScript.doorsOpen; // Toggle the door state
        }
        
    }

    public void OnLookEnter()
    {
        if (buttonFloor != 0)
        {
            FindAnyObjectByType<HUDManager>().CrossHairText.SetActive(true);
            FindAnyObjectByType<HUDManager>().UpdateCrossHairText("Press E to call elevator to floor " + buttonFloor);
        }
        else
        {
            FindAnyObjectByType<HUDManager>().CrossHairText.SetActive(true);
            FindAnyObjectByType<HUDManager>().UpdateCrossHairText("Press E to call elevator");
        }
        isLookedAt = true;
    }

    public void OnLookExit()
    {
        FindAnyObjectByType<HUDManager>().CrossHairText.SetActive(false);
        isLookedAt = false;
    }
}
