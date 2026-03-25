using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class HandFeetMinigameManager : MonoBehaviour
{
    public List<GameObject> limbs = new List<GameObject>();
    public List<GameObject> mazes = new List<GameObject>();
    public List<GameObject> pointerHolder = new List<GameObject>();
    public List<GameObject> endpoints = new List<GameObject>();
    public int currentMaze = 0;

    private Vector3 position;
    public float offset = 3f;

    RaycastHit hit;
    Ray ray;

    public MouseFollower mouseFollower;
    public GameObject raycastHolder;

    public float score = 100f;
    public float pointDeduction = 0.2f;

    public bool minigameEnd = false;

    public void Start()
    {
        currentMaze = Random.Range(0, 3);
        limbs[Random.Range(0, 3)].SetActive(true);
        mazes[currentMaze].SetActive(true);

        mouseFollower.transform.position = pointerHolder[currentMaze].transform.position;
    }

    public void Update()
    {
        position = Input.mousePosition;
        position.z = offset;

        if (mouseFollower.follow && !minigameEnd)
        {
            Debug.DrawRay(raycastHolder.transform.position, Vector3.forward * 5, Color.green);
            if (Physics.Raycast(raycastHolder.transform.position, Vector3.forward * 5, out hit))
            {
                if (hit.collider.gameObject.name == endpoints[currentMaze].gameObject.name)
                {
                    EndMinigame();
                }
            }
            else
            {
                score -= pointDeduction;
                if (score < 0)
                {
                    score = 0;
                }
            }
        }
    }

    private void EndMinigame()
    {
        minigameEnd = true;
        score = Mathf.Round(score);
        Debug.Log(score);
    }

}
