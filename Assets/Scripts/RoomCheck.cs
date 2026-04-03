using UnityEngine;

public class RoomCheck : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            GameManager.Instance.isInHotelRoom = true;
            GameManager.Instance.Body.Highlight(GameManager.Instance.GetOrganFromSlot(FindAnyObjectByType<InventoryController>().selectedIndex.Value - 1));
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other)
        {
            GameManager.Instance.isInHotelRoom = false;
            GameManager.Instance.Body.RemoveHighlight();
        }
    }
}
