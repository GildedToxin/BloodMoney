using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    [SerializeField] private List<GameObject> waypoints;
    private NavMeshAgent agent;

    private Vector3 target;
    private Vector3 currentPos;
    private int waypointSelect = 0;
    private bool canMove = true;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = waypoints[0].transform.position;
    }


    void Update()
    {
        currentPos = this.transform.position;
        if (currentPos.x != target.x && currentPos.z != target.z)
            agent.SetDestination(target); // Moves the npc towards the next waypoint
        else if (currentPos.x == target.x && currentPos.z == target.z && canMove)
        {
            waypointSelect++;
            try
            {
                target = waypoints[waypointSelect].transform.position; // Sets the new target waypoint after reaching target
            }
            catch
            {
                // For now made it so they cannot move once they reach their target
                // In the future will make it so that they teleport or get destroyed once they reach their target
                canMove = false;
            }
        }
    }
}
