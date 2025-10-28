using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject itemShopContent;
    public GameObject toolShopContent;
    public InventoryController inventoryController;
    public GameObject playerMoney;


    public GameObject itemShop;
    public GameObject toolShop;
    private void Awake()
    {
        inventoryController = FindAnyObjectByType<InventoryController>();
    }
    void Start()
    {

        RefreshShop(itemShopContent);
        RefreshShop(toolShopContent);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            RefreshShop(itemShopContent);
            RefreshShop(toolShopContent);
        }
    }

    public void RefreshShop(GameObject shop)
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in shop.transform)
            children.Add(child);

        children.Sort((a, b) =>
        {
            ShopItem itemA = a.GetComponent<ShopItem>();
            ShopItem itemB = b.GetComponent<ShopItem>();

            if (itemA.isPurchased != itemB.isPurchased)
                return itemA.isPurchased.CompareTo(itemB.isPurchased);

            return int.Parse(itemA.itemPrice.GetComponent<TextMeshProUGUI>().text).CompareTo(int.Parse(itemB.itemPrice.GetComponent<TextMeshProUGUI>().text));
        });

        for (int i = 0; i < children.Count; i++)
            children[i].SetSiblingIndex(i);
    }
    public bool TryToBuyItem(ShopItem shopItem)
    {
        if(inventoryController.money >= shopItem.item.price && !shopItem.isPurchased)
        {
            if (inventoryController.TryAddItem(shopItem.item))
            {
                inventoryController.money.Value -= shopItem.item.price;
                playerMoney.GetComponent<TextMeshProUGUI>().SetText(inventoryController.money.ToString());
                return true;
            }
            return false;
        }
        return false;
    }
    public void OnTabClicked(GameObject tab)
    {
        if (tab == itemShop)
        {
            toolShop.SetActive(false);
            itemShop.SetActive(true);
        }
        else
        {
            itemShop.SetActive(false);
            toolShop.SetActive(true);
        }
    }
    public void CloseShop()
    {
        gameObject.SetActive(false);
    }
}
