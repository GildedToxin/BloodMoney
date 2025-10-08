using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEngine.AI;

public class HuntNPCController : MonoBehaviour
{
    [SerializeField] private List<GameObject> waypoints;
    private NavMeshAgent agent;
    [SerializeField] private HuntVisionDetection visionTrigger;
    private Vector3 huntTarget;

    private Vector3 targetWaypoint;
    private Vector3 currentPos;
    private int waypointSelect = 0;
    private bool canMove = true;
    private bool chasePlayer = false;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        targetWaypoint = waypoints[0].transform.position;
    }

    void Update()
    {
        currentPos = this.transform.position;

        if (visionTrigger.foundTarget == true)  // Sets variable for huntTarget once the player is visible
        {
            try
            {
                huntTarget = visionTrigger.target.position;
                chasePlayer = true;
            }
            catch
            {
                print("Unable to find player");
                chasePlayer = false;
            }
        }


        if (chasePlayer == true && huntTarget != null)  // Sets the nav agent target to visible player
            agent.SetDestination(huntTarget);
        else
            MoveToWaypoint();

        SelectWaypoint();
    }

    /// <summary>
    /// Moves to the target waypoint selected with the SelectWaypoint function
    /// </summary>
    private void MoveToWaypoint()
    {
        if (currentPos.x != targetWaypoint.x && currentPos.z != targetWaypoint.z)
            agent.SetDestination(targetWaypoint); // Moves the npc towards the next waypoint
    }

    /// <summary>
    /// Automatically selects the next waypoint in the waypoint list when the current position is on the target position
    /// </summary>
    private void SelectWaypoint()
    {
        if (currentPos.x == targetWaypoint.x && currentPos.z == targetWaypoint.z && canMove)
        {
            waypointSelect++;
            try
            {
                targetWaypoint = waypoints[waypointSelect].transform.position; // Sets the new target waypoint after reaching target
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
