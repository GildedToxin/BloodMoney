using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public HUDManager inventoryUIManager;

    [HideInInspector] public bool isHovered;
    private UnityEngine.UI.Image itemIcon;
    //private UnityEngine.UI.Image startingIcon;

    public Item item;
    public string itemName;
    public bool isSelected;


    private void Awake()
    {
        //startingIcon = transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();
        itemIcon = transform.GetComponent<UnityEngine.UI.Image>();
        
    }
    void Start()
    {
            InitalizeItem();
    }
    public void InitalizeItem()
    {
        if (item != null)
        {
            itemIcon.GetComponent<UnityEngine.UI.Image>().sprite = item.icon;
            itemName = item.name;
        }

        else
        {
            itemIcon.GetComponent<UnityEngine.UI.Image>().sprite = itemIcon.sprite;
            print("item Name failed");
        }
    }
    public void ClearItem()
    {
        itemIcon.GetComponent<UnityEngine.UI.Image>().sprite = itemIcon.sprite;
        item = null;
    }
    public void SetItem(Item newItem)
    {
        item = newItem;
        InitalizeItem();
    }

}
