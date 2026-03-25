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

    public float score = 100f;
    public float pointDeduction = 0.2f; 

    public void Update()
    {
        position = Input.mousePosition;
        position.z = offset;

        if (mouseFollower.follow)
        {
            Debug.DrawRay(raycastHolder.transform.position, Vector3.forward * 5, Color.green);
            if (Physics.Raycast(raycastHolder.transform.position, Vector3.forward * 5, out hit))
            {

            }
            else
            {
                score -= pointDeduction;
                Debug.Log(score);
            }
        }
    }

}
