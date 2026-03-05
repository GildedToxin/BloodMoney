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

    Ray ray;
    RaycastHit hit;

    public MouseFollower mouseFollower;

    public void Update()
    {
        position = Input.mousePosition;
        ray = Camera.main.ScreenPointToRay(position);
        position.z = offset;

        if (mouseFollower.follow)
        {
            if (Physics.Raycast(ray, out hit) && hit.collider != mazes[currentMaze])
            {
                Debug.Log("miss");
            }
        }
    }

}
