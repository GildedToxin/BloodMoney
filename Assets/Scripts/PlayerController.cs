using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public InventoryUIManager inventoryUIManager;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            inventoryUIManager.gameObject.SetActive(!inventoryUIManager.gameObject.activeSelf);
        }
    }
}
