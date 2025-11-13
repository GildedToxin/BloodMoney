using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("References")]
    public Item item;
    public Image itemIcon;
    public GameObject itemName;
    public GameObject itemPrice;

    [Header("State")]
    public bool isPurchased = false;

    [Header("Colors")]
    public Color purchasedColor = Color.gray;
    public Color defaultColor;
    public Color hoverColor;


    public ShopManager shopManager;
    

    public void Start()
    {
        InitalizeShopItem();
        if(shopManager == null)
            shopManager = FindAnyObjectByType<ShopManager>();
    }
    public void OnBuyPressed()
    {
        if (shopManager.TryToBuyItem(shopItem: this))
        {
            isPurchased = true;
            GetComponent<Image>().color = purchasedColor;
            shopManager.RefreshShop(ShopManager.Instance.itemShopContent);
            shopManager.RefreshShop(ShopManager.Instance.toolShopContent);
            FindAnyObjectByType<InventoryController>().SubtractMoney(item.price);
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
           // print(item.price.ToString());
        }
        if (isPurchased)
            GetComponent<Image>().color = purchasedColor;
    }
}
