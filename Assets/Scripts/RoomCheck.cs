using UnityEngine;

public class RoomCheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.isInHotelRoom = true;
        }
    }
    private void OntriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.isInHotelRoom = false;
        }
    }
}
