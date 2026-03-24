using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class HandFeetMinigameManager : MonoBehaviour
{
    public List<GameObject> limbs = new List<GameObject>();
    public List<GameObject> mazes = new List<GameObject>();
    public int currentMaze = 0;

    private Vector3 position;
    public float offset = 3f;

    RaycastHit hit;
    Ray ray;

    public MouseFollower mouseFollower;
    public GameObject raycastHolder;

    public void Update()
    {
        position = Input.mousePosition;
        position.z = offset;

        if (mouseFollower.follow)
        {
            Debug.DrawRay(raycastHolder.transform.position, Vector3.forward * 20, Color.green);
            if (Physics.Raycast(raycastHolder.transform.position, Vector3.forward * 1000, out hit) && hit.collider.gameObject == mazes[currentMaze])
            {
                Debug.Log("hit " + hit.collider);
            }
        }
    }

}
