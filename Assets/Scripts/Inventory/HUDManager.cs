using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class HUDManager : MonoBehaviour
{
    public GameObject draggableItemGO;
    public GameObject InventoryGrid;
    public Color defaultColor;
    public Color selectedColor;
    public InventoryController inventoryController;
    public GameObject CrossHairText;

    public GameObject timerText;
    public GameObject moneyText;
    public GameObject dayText;
    public GameObject bookPanel;
    public Sprite blankIcon;

    public GameObject bookClosed;
    public GameObject bookOpen;

    public GameObject roomNumber;

    public GameObject bloodSelection;
    private void Awake()
    {
        inventoryController = FindAnyObjectByType<InventoryController>();
    }
    void Start()
    {
        foreach(Transform child in InventoryGrid.transform)
        {
            if(child.GetComponent<InventorySlot>())
                child.GetComponent<InventorySlot>().inventoryUIManager = this;
        }
        RefreshUI();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            //FindAnyObjectByType<InventoryController>().money.Value += 10;
            bookPanel.SetActive(!bookPanel.activeSelf);
            bookClosed.SetActive(!bookClosed.activeSelf);
            bookOpen.SetActive(!bookOpen.activeSelf);
        }
    }

  
    public void RefreshUI() {
        foreach (Transform child in InventoryGrid.transform)
        {
            foreach (var pair in inventoryController.items)
            {
                if(!child.GetComponent<InventorySlot>()) 
                    continue;
                if (pair.Key == child.GetComponent<InventorySlot>().itemName)
                {
                    if (pair.Value) 
                    {
                        child.transform.GetComponent<Image>().sprite = child.GetComponent<InventorySlot>().item.icon;
                        break;
                    }
                }
                else {
                    child.transform.GetComponent<Image>().sprite = blankIcon; //gameObject.SetActive(false);

                }
            }
        }
        moneyText.GetComponent<TextMeshProUGUI>().text = $"${inventoryController.money.Value}";
        dayText.GetComponent<TextMeshProUGUI>().text = $"Day {FindAnyObjectByType<GameManager>().currentDay}/30";
    }

    public void RefreshUISelection(int selection)
    {
        if (selection >= 0 && selection < 7) 
        { 
            foreach (Transform child in InventoryGrid.transform)
            {
                //child.GetChild(0).gameObject.SetActive(false);
            }
            bloodSelection.transform.position = new Vector3(InventoryGrid.transform.GetChild(selection).position.x, bloodSelection.transform.position.y, bloodSelection.transform.position.z);
            //InventoryGrid.transform.GetChild(selection).GetChild(0).gameObject.SetActive(true);
        }
        RefreshUI();
    }

    public void UpdateSellAsk(string desiredItem)
    {
        CrossHairText.GetComponent<TextMeshProUGUI>().text = $"Press Q to sell {desiredItem}";
    }
    private void OnEnable()
    {
            //inventoryController.inventory.OnValueChanged  += (i, item) => RefreshUI();
            inventoryController.selectedIndex.OnValueChanged += (i) => RefreshUISelection(i - 1);
            inventoryController.money.OnValueChanged += (i) => RefreshUI();
    }
    private void OnDisable()
    {
        //inventoryController.inventory.OnValueChanged -= (i, item) => RefreshUI();
        inventoryController.selectedIndex.OnValueChanged -= (i) => RefreshUISelection(i - 1);
        inventoryController.money.OnValueChanged -= (i) => RefreshUI();
    }
    public void UpdateCrossHairText(string newText)
    {
        CrossHairText.GetComponent<TextMeshProUGUI>().text = newText;
    }
    public void UpdateRoomNumber(string roomNumber)
    {
        this.roomNumber.GetComponent<TextMeshProUGUI>().text = $"{roomNumber}";
    }
}
