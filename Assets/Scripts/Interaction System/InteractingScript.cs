using UnityEngine;

public class InteractingScript : MonoBehaviour
{
    // tracks what is interactable and how the range in which the player can interact with them
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private float interactionPointRadius = 0.5f;
    [SerializeField] private LayerMask interactableMask;

    // Number of items the script will search for
    private readonly Collider[] colliders = new Collider[3];
    [SerializeField] private int numFound;

    private void Update()
    {
        //finds everything interactable within the interaction point radius into the collider array and return the number of things found as an int
        numFound = Physics.OverlapSphereNonAlloc(interactionPoint.position, interactionPointRadius, colliders, interactableMask);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interactionPoint.position, interactionPointRadius);
    }


}
