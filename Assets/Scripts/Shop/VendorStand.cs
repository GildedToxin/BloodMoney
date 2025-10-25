using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VendorStand : MonoBehaviour
{
    public BoxCollider sellingPoint;
    public List<OrganManager> organsToSell;
    public List<Customer> customers; 

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<OrganManager>())
            return;
        if (organsToSell.Contains(other.GetComponent<OrganManager>()))
            return;
    
        organsToSell.Add(other.GetComponent<OrganManager>());

        CheckToSell(other.GetComponent<OrganManager>());
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.GetComponent<OrganManager>())
            return;
        if (!organsToSell.Contains(other.GetComponent<OrganManager>()))
            return;

        organsToSell.Remove(other.GetComponent<OrganManager>());
    }

    public void CreateCustomers()
    {

    }

    public void CheckToSell(OrganManager organ)
    {
        foreach(Customer customer in customers)
            if(organ.organType == customer.desiredOrgan)
                customer.canBuyItem = true;

    }


    public void UpdateVenders()
    {
        //CreateCustomers();
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

        FindAnyObjectByType<PlayerController>().AddMoney(organ.GetOrganPrice());

        organsToSell.Remove(organ);
        Destroy(organ.gameObject);

        customers.Remove(customer);
        Destroy(customer.gameObject);

        UpdateVenders();
    }

}
