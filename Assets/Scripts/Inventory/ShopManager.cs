using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject shopContent;
    public InventoryController inventoryController;
    public GameObject playerMoney;
    private void Awake()
    {
        inventoryController = FindAnyObjectByType<InventoryController>();
    }
    void Start()
    {

        RefreshShop();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            RefreshShop();
        }
    }

    public void RefreshShop()
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in shopContent.transform)
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
    private void OnEnable()
    {
        playerMoney.GetComponent<TextMeshProUGUI>().SetText(inventoryController.money.ToString());
    }
}
