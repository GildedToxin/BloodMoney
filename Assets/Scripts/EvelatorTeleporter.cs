using UnityEngine;
using System.Collections.Generic;

public class EvelatorTeleporter : MonoBehaviour
{
    public Transform currentBox;
    public Transform box1;
    public Transform box2;
    public Transform box3;
    public Collider collider;

    public List<GameObject> listOfObjects = new List<GameObject>();

    //private bool inBox1 = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            foreach (GameObject obj in listOfObjects)
            {
                try
                {
                    Teleport(currentBox, box1, obj);
                }
                catch
                {
                    Debug.Log("Teleport to box1 failed");;
                }
                
            }
     
        }
        Debug.Log(listOfObjects.Count);
    }

    void Teleport(Transform fromBox, Transform toBox, GameObject obj)
    {
        // Get the player's position relative to the current box
        Vector3 localPos = fromBox.InverseTransformPoint(obj.transform.position);

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
