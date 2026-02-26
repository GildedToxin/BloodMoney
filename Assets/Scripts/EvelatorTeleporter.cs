using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;

public class EvelatorTeleporter : MonoBehaviour
{
    public bool buttonPressed = false;

    public static bool isMoving = false;

    public int currentFloor;
    public int targetFloor;
    public bool doorsOpen = false;
    public GameObject rightDoor;
    public GameObject leftDoor;
    private Vector3 rightDoorOpenPos;
    private Vector3 rightDoorClosedPos;
    private Vector3 leftDoorOpenPos;
    private Vector3 leftDoorClosedPos;
    public List<GameObject> listOfElevatorPlatforms = new List<GameObject>();
    public List<GameObject> listOfObjects = new List<GameObject>();
    public List<GameObject> listOfEvelatorTeleporters = new List<GameObject>();

    private bool isTeleporting = false;

    public bool hasShownScreen;
    public bool hasShownScreen2;
    void Start()
    {
        rightDoorOpenPos = rightDoor.transform.position;
        leftDoorOpenPos = leftDoor.transform.position;
        rightDoorClosedPos = rightDoor.transform.position + new Vector3(0, 0, 1.05f);
        leftDoorClosedPos = leftDoor.transform.position - new Vector3(0, 0, 1.05f);
        rightDoor.transform.position = rightDoorClosedPos;
        leftDoor.transform.position = leftDoorClosedPos;
    }

    void FixedUpdate()
    {
        if (buttonPressed == true && targetFloor != currentFloor)
        {
            doorsOpen = false;
            StartCoroutine(TeleportWithDelay(1f));
            buttonPressed = false;
        }
        else if (buttonPressed == true && targetFloor == currentFloor)
            buttonPressed = false;

        OpenCloseDoors();
    }

    void Teleport(int current, int target, GameObject obj)//Transform fromBox, Transform toBox, GameObject obj)
    {

        Transform fromBox = listOfElevatorPlatforms[current - 1].transform;

        // Get the player's position relative to the current box
        Vector3 localPos = fromBox.InverseTransformPoint(obj.transform.position);

        Transform toBox = listOfElevatorPlatforms[target - 1].transform;

        // Convert that local position to world space in the new box
        Vector3 newWorldPos = toBox.TransformPoint(localPos);

        // Move player
        obj.transform.position = newWorldPos;
    }
    
    void OnTriggerEnter(Collider other)
    {
        print("test");

        if(GameManager.Instance.currentDay == 0 && !hasShownScreen && FindAnyObjectByType<FirstDayManager>().currentScreen == 6)
        {
            var fdm = FindAnyObjectByType<FirstDayManager>();
            fdm.currentScreen++; //7
            fdm.isShowingScreen = true;
            fdm.tutorialScreens[fdm.currentScreen].SetActive(true);
            hasShownScreen = true;
            StartCoroutine(fdm.WaitForNextScreen());
        }
        //compare tag
        if (other.gameObject.tag == "Teleportable")
        {
            listOfObjects.Add(other.gameObject);
            
            if (other.gameObject.GetComponent<PlayerController>() && FindAnyObjectByType<CartBehavior>().moveing == true)
            {
                listOfObjects.Add(FindAnyObjectByType<CartBehavior>().gameObject);
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        listOfObjects.Remove(other.gameObject);

    
            
            if (other.gameObject.GetComponent<PlayerController>() && FindAnyObjectByType<CartBehavior>().moveing == true)
            {
                listOfObjects.Remove(FindAnyObjectByType<CartBehavior>().gameObject);
            }

    }

    public IEnumerator WaitOneSecond()
    {
        yield return new WaitForSeconds(1);
        isMoving = false;
    }

    public void OpenCloseDoors()
    {
        if (doorsOpen == false)
        {
            rightDoor.transform.position = Vector3.Lerp(rightDoor.transform.position, rightDoorClosedPos, Time.deltaTime * 2);
            leftDoor.transform.position = Vector3.Lerp(leftDoor.transform.position, leftDoorClosedPos, Time.deltaTime * 2);
        }
        else
        {
            rightDoor.transform.position = Vector3.Lerp(rightDoor.transform.position, rightDoorOpenPos, Time.deltaTime * 2);
            leftDoor.transform.position = Vector3.Lerp(leftDoor.transform.position, leftDoorOpenPos, Time.deltaTime * 2);
        }
    }

    public void QueueTeleport()
    {
        foreach (GameObject obj in listOfObjects)
        {
            try
            {
                Teleport(currentFloor, targetFloor, obj);
            }
            catch
            {
                Debug.Log("Teleport to box" + targetFloor + " failed");
            }
        }
        GameObject targetTeleporter = listOfEvelatorTeleporters[targetFloor - 1];
        targetTeleporter.GetComponent<EvelatorTeleporter>().doorsOpen = true;
    }

    public IEnumerator TeleportWithDelay(float seconds)
    {
        float timer = 0f;
        while (timer < seconds)
        {
            timer += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        foreach (GameObject obj in listOfObjects)
        {
            try
            {
                Teleport(currentFloor, targetFloor, obj);
            }
            catch
            {
                Debug.Log("Teleport to box" + targetFloor + " failed");
            }
        }
        listOfObjects.Clear();
        GameObject targetTeleporter = listOfEvelatorTeleporters[targetFloor - 1];
        targetTeleporter.GetComponent<EvelatorTeleporter>().doorsOpen = true;

        if (GameManager.Instance.currentDay == 0 && !hasShownScreen2 && FindAnyObjectByType<FirstDayManager>().currentScreen == 7)
        {
            var fdm = FindAnyObjectByType<FirstDayManager>();
            fdm.currentScreen++; //8
            fdm.isShowingScreen = true;
            fdm.tutorialScreens[fdm.currentScreen].SetActive(true);
            hasShownScreen2 = true;
            StartCoroutine(fdm.WaitForNextScreen());
        }
    }
}
