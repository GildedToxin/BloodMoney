using UnityEngine;

public class HuntVisionDetection : MonoBehaviour
{
    [HideInInspector] public Transform target;
    [HideInInspector] public bool foundTarget = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            target = other.transform;
            foundTarget = true;
        }
        else
        {
            target = null;
            foundTarget = false;
        }

    }
}
