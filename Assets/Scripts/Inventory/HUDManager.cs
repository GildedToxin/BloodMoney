using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

public class HUDManager : MonoBehaviour
{
    public GameObject draggableItemGO;
    public GameObject InventoryGrid;
    public Color defaultColor;
    public Color selectedColor;
    public InventoryController inventoryController;
    public GameObject sellItem;

    public GameObject timerText;
    public GameObject moneyText;
    public GameObject dayText;
    public GameObject bookPanel;

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
        RefreshUI();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            FindAnyObjectByType<InventoryController>().money.Value += 10;
            bookPanel.SetActive(!bookPanel.activeSelf);
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
    public void RefreshUI() {         
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
        moneyText.GetComponent<TextMeshProUGUI>().text = $"${inventoryController.money.Value}";
        dayText.GetComponent<TextMeshProUGUI>().text = $"Day {FindAnyObjectByType<GameManager>().currentDay}/30";

    }
    public void UpdateSellAsk(string desiredItem)
    {
        sellItem.GetComponent<TextMeshProUGUI>().text = $"Press Q to sell {desiredItem}";
    }
    private void OnEnable()
    {
            inventoryController.inventory.OnValueChanged  += (i, item) => RefreshUI();
            inventoryController.selectedIndex.OnValueChanged += (i) => RefreshUI();
            inventoryController.money.OnValueChanged += (i) => RefreshUI();
    }
    private void OnDisable()
    {
        inventoryController.inventory.OnValueChanged -= (i, item) => RefreshUI();
        inventoryController.selectedIndex.OnValueChanged -= (i) => RefreshUI();
        inventoryController.money.OnValueChanged -= (i) => RefreshUI();
    }
}
