using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Item item;
    public GameObject itemIcon;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       if(item!=null)
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
    public Item GetItem()
    {
        return item;
    }
    public void SetItem(Item newItem)
    {
        item = newItem;
        InitalizeItem();
    }
}
