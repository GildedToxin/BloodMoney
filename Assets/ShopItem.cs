using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image itemIcon;
    public GameObject itemName;
    public GameObject itemPrice;


    public bool isPurchased = false;
    public Color purchasedColor = Color.gray;
    public Color defaultColor;
    public Color hoverColor;
    public ShopManager shopManager;
    public Item item;

    public void Start()
    {
        InitalizeShopItem();
    }
    public void OnBuyPressed()
    {
        if (shopManager.TryToBuyItem(shopItem: this))
        {
            isPurchased = true;
            GetComponent<Image>().color = purchasedColor;
            shopManager.RefreshShop();
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isPurchased)
            GetComponent<Image>().color = hoverColor;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isPurchased)
            GetComponent<Image>().color = defaultColor;
    }

    private void InitalizeShopItem()
    {
        if(item != null)    
        {
            itemIcon.sprite = item.icon;
            itemName.GetComponent<TextMeshProUGUI>().SetText(item.name);
            itemPrice.GetComponent<TextMeshProUGUI>().SetText(item.price.ToString());
        }
        if (isPurchased)
            GetComponent<Image>().color = purchasedColor;
    }
}
