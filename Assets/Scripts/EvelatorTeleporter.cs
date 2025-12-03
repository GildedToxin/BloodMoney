using UnityEngine;
using System.Collections.Generic;

public class EvelatorTeleporter : MonoBehaviour
{
    public bool buttonPressed = false;

    public int currentFloor;
    public int targetFloor;
    public List<GameObject> listOfElevatorPlatforms = new List<GameObject>();
    public List<GameObject> listOfObjects = new List<GameObject>();

    void Update()
    {
        if (buttonPressed == true && targetFloor != currentFloor)
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
            buttonPressed = false;
        }
        else if (buttonPressed == true && targetFloor == currentFloor)
            buttonPressed = false;

        Debug.Log(listOfObjects.Count);
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
        //compare tag
        if (other.gameObject.tag == "Teleportable")
            listOfObjects.Add(other.gameObject);
    }
    void OnTriggerExit(Collider other)
    {
        listOfObjects.Remove(other.gameObject);
    }
}
