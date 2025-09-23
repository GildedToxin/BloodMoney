using UnityEngine;
using static UnityEditor.Progress;

public class InventoryUIManager : MonoBehaviour
{
    public GameObject draggableItemGO;
    public GameObject InventoryGrid;
    public Color defaultColor;
    public Color selectedColor;
    public InventoryController inventoryController;

    private void Awake()
    {
        inventoryController = FindAnyObjectByType<InventoryController>();
    }
    void Start()
    {
        foreach(Transform child in InventoryGrid.transform)
        {
            child.GetComponent<InventorySlot>().inventoryUIManager = this;
        }
    }

    public bool DropItemAtNewSlot(Item item, InventorySlot oldSlot)
    {
        int index = -1; 
        foreach (Transform child in InventoryGrid.transform)
        {
            index++;
            InventorySlot slot = child.GetComponent<InventorySlot>();
            if (slot.isHovered && slot.item == null)
            {
                inventoryController.TryAddItemAtIndex(item, index);
                inventoryController.TryRemoveItemAtIndex(oldSlot.transform.GetSiblingIndex());
                return true;
            }
        }
        return false;
    }

    public void RefreshInventory() {         
        for (int i = 0; i < inventoryController.inventory.Length; i++)
        {
            InventorySlot slot = transform.GetChild(0).GetChild(i).GetComponent<InventorySlot>();
            Item item = inventoryController.GetItemAtIndex(i);
            if (item != null)
            {
                slot.SetItem(item);
            }
            else
            {
                slot.ClearItem();
            }
            if(i == inventoryController.selectedIndex -1)
            {
                print(i);   
                slot.GetComponent<UnityEngine.UI.Image>().color = selectedColor;
            }
            else
            {
                slot.GetComponent<UnityEngine.UI.Image>().color = defaultColor;
            }
        }
    }
    private void OnEnable()
    {
        try
        {
            inventoryController.inventory.OnValueChanged  += (i, item) => RefreshInventory();
            inventoryController.selectedIndex.OnValueChanged += (i) => RefreshInventory();
            RefreshInventory();
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error refreshing inventory: {ex.Message}");
        }
    }
    private void OnDisable()
    {
        inventoryController.inventory.OnValueChanged -= (i, item) => RefreshInventory();
        inventoryController.selectedIndex.OnValueChanged -= (i) => RefreshInventory();
    }
}
