using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class InventorySlot : MonoBehaviour
{
    public InventoryUIManager inventoryUIManager;

    [HideInInspector] public bool isHovered;
    private UnityEngine.UI.Image itemIcon;


    public Item item;


    private void Awake()
    {
        itemIcon = transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();
    }
    void Start()
    {
        if (item != null)
            InitalizeItem();
    }
    public void InitalizeItem()
    {
        itemIcon.GetComponent<UnityEngine.UI.Image>().sprite = item.icon;
    }
    public void ClearItem()
    {
        itemIcon.GetComponent<UnityEngine.UI.Image>().sprite = null;
        item = null;
    }
    public void SetItem(Item newItem)
    {
        item = newItem;
        InitalizeItem();
    }

}
