using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class ShopManager : MonoBehaviour
{
    public GameObject itemShopContent;
    public GameObject toolShopContent;
    public InventoryController inventoryController;
    public GameObject playerMoney;


    public GameObject itemShop;
    public GameObject toolShop;

    public static ShopManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
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

    }

    public void RefreshShop(GameObject shop)
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in shop.transform)
            children.Add(child);

        //print(children);
        children.Sort((a, b) =>
        {
            ShopItem itemA = a.GetComponent<ShopItem>();
            ShopItem itemB = b.GetComponent<ShopItem>();

            if (itemA.isPurchased != itemB.isPurchased)
                return itemA.isPurchased.CompareTo(itemB.isPurchased);

            int priceA = 0;
            int priceB = 0;

            int.TryParse(itemA.itemPrice.GetComponent<TextMeshProUGUI>().text, out priceA);
            int.TryParse(itemB.itemPrice.GetComponent<TextMeshProUGUI>().text, out priceB);

            return priceA.CompareTo(priceB);
        });

        for (int i = 0; i < children.Count; i++)
            children[i].SetSiblingIndex(i);
    }
    public bool TryToBuyItem(ShopItem shopItem)
    {
        if(inventoryController.money >= shopItem.item.price && !shopItem.isPurchased)
        {
            if(inventoryController.DoesPlayerHaveItem(shopItem.item.name))
                return false;

            //if()

                foreach (var pair in inventoryController.items)
                {
                    if (pair.Key == shopItem.item.name)
                    {
                        inventoryController.items[shopItem.item.name] = true;
                        FindAnyObjectByType<HUDManager>().RefreshUI();
                    return true;
                    }
                }  
        }
        return false;
    }
    public void OnTabClicked(GameObject tab)
    {
       // print(tab.name);
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
        transform.GetChild(0).gameObject.SetActive(false);
        Camera.main.GetComponent<CameraMovement>().CloseUI();
        FindAnyObjectByType<HUDManager>().UpdateCrossHairText("Press E to Open Shop");
    }
}
