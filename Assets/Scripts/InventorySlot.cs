using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IEndDragHandler, IBeginDragHandler//, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public Color hoverColor;
    public Color defaultColor = new Color(0,0,0,1);
    public InventoryUIManager inventoryUIManager;

    [HideInInspector] public bool isHovered;
    private UnityEngine.UI.Image itemIcon;

    private bool isDragging = false;

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
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        GetComponent<UnityEngine.UI.Image>().color = hoverColor;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        GetComponent<UnityEngine.UI.Image>().color = defaultColor;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            isDragging = true;
            itemIcon.color = new Color(itemIcon.color.r, itemIcon.color.g, itemIcon.color.b, .05f);
            inventoryUIManager.draggableItemGO.SetActive(true);
            inventoryUIManager.draggableItemGO.GetComponent<UnityEngine.UI.Image>().sprite = item.icon;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
            inventoryUIManager.draggableItemGO.transform.position = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            if (isHovered) return;
            if (isDragging && inventoryUIManager.DropItemAtNewSlot(item, oldSlot: this))
            {
                ClearItem();
            }
        }

        itemIcon.color = new Color(itemIcon.color.r, itemIcon.color.g, itemIcon.color.b, 1f);
        inventoryUIManager.draggableItemGO.SetActive(false);
        isDragging = false;
    }
    private void OnDisable()
    {
        isDragging = false;
        GetComponent<UnityEngine.UI.Image>().color = defaultColor;
        itemIcon.color = new Color(itemIcon.color.r, itemIcon.color.g, itemIcon.color.b, 1f);
        inventoryUIManager.draggableItemGO.SetActive(false);
    }

}
