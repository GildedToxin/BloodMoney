using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VendorStand : MonoBehaviour
{
    public BoxCollider sellingPoint;
    public List<OrganManager> organsToSell;
    public List<Customer> customers;

    public bool hasShownScreen;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponentInParent<OrganManager>())
            return;
        if (organsToSell.Contains(other.GetComponentInParent<OrganManager>()))
            return;

        organsToSell.Add(other.GetComponentInParent<OrganManager>());

        CheckToSell(other.GetComponentInParent<OrganManager>());
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.GetComponentInParent<OrganManager>())
            return;
        if (!organsToSell.Contains(other.GetComponentInParent<OrganManager>()))
            return;

        organsToSell.Remove(other.GetComponentInParent<OrganManager>());
    }

    public void CreateCustomers()
    {

    }

    public void CheckToSell(OrganManager organ)
    {
        foreach (Customer customer in customers)
        {
            if (organ.organType == customer.desiredOrgan)
                customer.canBuyItem = true;
            print(organ.organType.ToString() + " vs " + customer.desiredOrgan.ToString() + " = " + customer.canBuyItem.ToString());
        }

    }


    public void UpdateVenders()
    {
        //CreateCustomers();
        foreach (Customer customer in customers)
            customer.canBuyItem = false;

        foreach(OrganManager organ in organsToSell)
            CheckToSell(organ);
    }

    public void SellOrgan(OrganType organType, Customer customer)
    {
        // Find the organ to sell
        OrganManager organ = organsToSell.FirstOrDefault(o => o.organType == organType);

        if (organ == null)
        {
            Debug.LogWarning($"No organ of type {organType} found to sell!");
            return;
        }

        Debug.Log($"Sold {organ.organType} to {customer.customerType}!");

        FindAnyObjectByType<InventoryController>().AddMoney(organ.GetOrganPrice());

        if(organ.gameObject == FindAnyObjectByType<HeldItem>().currentItem)
        {
            FindAnyObjectByType<HeldItem>().DropItem(organ.gameObject);
        }
        organsToSell.Remove(organ);
        Destroy(organ.gameObject);

        customers.Remove(customer);
        customer.gameObject.SetActive(false);
        //Destroy(customer.gameObject);

        UpdateVenders();

        if (GameManager.Instance.currentDay == 0 && !hasShownScreen && FindAnyObjectByType<FirstDayManager>().currentScreen == 14)
        {
            var fdm = FindAnyObjectByType<FirstDayManager>();
            fdm.currentScreen++; //15
            fdm.isShowingScreen = true;
            fdm.tutorialScreens[fdm.currentScreen].SetActive(true);
            hasShownScreen = true;
            StartCoroutine(fdm.WaitForNextScreen());
        }
    }

    public void RefreshCustomers()
    {
        FindObjectsByType<Customer>( FindObjectsInactive.Include, FindObjectsSortMode.None).ToList().ForEach(c =>
        {
            c.gameObject.SetActive(true);
            customers.Add(c);
            c.isServed = false;
        });
    }

}
