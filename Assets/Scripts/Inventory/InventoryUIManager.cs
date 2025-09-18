using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    public GameObject draggableItemGO;
    public GameObject InventoryGrid;
    public InventoryController inventoryController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        inventoryController = FindAnyObjectByType<InventoryController>();
    }
    void Start()
    {
        foreach(Transform child in InventoryGrid.transform)
        {
            child.GetComponent<UIElement>().inventoryUIManager = this;
        }
    }

    public bool TryAddItemToSlot(Item item)
    {
        foreach (Transform child in InventoryGrid.transform)
        {
            InventorySlot slot = child.GetComponent<InventorySlot>();
            if (slot.GetComponent<UIElement>().isHovered && slot.item == null)
            {
                slot.SetItem(item);
                return true;
            }
        }
        return false;
    }
}
