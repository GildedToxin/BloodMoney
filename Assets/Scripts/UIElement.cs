using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public InventorySlot inventorySlot;
    public Color hoverColor;
    public Color defaultColor = new Color(0,0,0,1);
    public InventoryUIManager inventoryUIManager;

    [HideInInspector] public bool isHovered;
    private UnityEngine.UI.Image itemIcon;

    private bool isDragging = false;

    private void Awake()
    {
        itemIcon = transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        GetComponent<UnityEngine.UI.Image>().color = hoverColor;
        if (inventorySlot.GetItem() != null)
            print(inventorySlot.GetItem());
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        GetComponent<UnityEngine.UI.Image>().color = defaultColor;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (inventorySlot.GetItem() != null)
        {
            isDragging = true;
            itemIcon.color = new Color(itemIcon.color.r, itemIcon.color.g, itemIcon.color.b, .05f);
            inventoryUIManager.draggableItemGO.SetActive(true);
            inventoryUIManager.draggableItemGO.GetComponent<UnityEngine.UI.Image>().sprite = inventorySlot.GetItem().icon;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (inventorySlot.GetItem() != null)
            inventoryUIManager.draggableItemGO.transform.position = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (inventorySlot.GetItem() != null)
        {
            
            if (!isHovered && isDragging)
            {
                if (inventoryUIManager.TryAddItemToSlot(inventorySlot.GetItem())) {
                    inventorySlot.ClearItem();
                }
            }
            else if (isHovered)
            {
                print("Dropped on self, do nothing");
            }
        }

        itemIcon.color = new Color(itemIcon.color.r, itemIcon.color.g, itemIcon.color.b, 1f);
        inventoryUIManager.draggableItemGO.SetActive(false);
        isDragging = false;
    }
    private void OnDisable()
    {
        isDragging = false;
        itemIcon.color = new Color(itemIcon.color.r, itemIcon.color.g, itemIcon.color.b, 1f);
        inventoryUIManager.draggableItemGO.SetActive(false);
    }

}
