using UnityEngine;

public class EyePositionScript : MonoBehaviour
{
    public EyeMinigameController emc;

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Scoop Icon")
        {
            emc.scoopInPosition = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Scoop Icon")
        {
            emc.scoopInPosition = false;
        }
    }
}
