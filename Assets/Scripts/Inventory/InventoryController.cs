using JetBrains.Annotations;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public InventorySlot[] inventorySlots;
    public bool TryAddItem(Item item)
    {
        
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.item == null)
            {
                slot.SetItem(item);
                return true;
            }
        }
        return false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (!TryAddItem(Resources.Load<Item>("Items/DebugItem")))
            {
                print("Failed to add item to inventory!");
            }
        }
    }
}
