using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public enum CustomerType
{
    Zombie,
    Skeleton,
    Witch,
}
public class Customer : MonoBehaviour, IPlayerLookTarget
{
    public CustomerType customerType;
    public OrganType desiredOrgan;

    public Canvas customerRequest;
    public GameObject Price;

    public bool isServed = false;
    public bool isLookedAt = false;
    public bool canBuyItem = false;
    public ParticleSystem blood;

    public string currentText;

    public OrganManager RequestedOrgan;
    private void Start()
    {
        RandomCustomer();
    }
    void Update()
    {
        if (isLookedAt && canBuyItem && !isServed)
        {
            if (Input.GetKeyDown(KeyCode.E)) {
                isServed = true;
                FindAnyObjectByType<VendorStand>().SellOrgan(desiredOrgan, this, RequestedOrgan);
            }

           
        } 
    }

    [ContextMenu("Random Customer")]
    public void RandomCustomer()
    {
        var oldCustomer = customerType;
        do {
            customerType = (CustomerType)Random.Range(0, System.Enum.GetValues(typeof(CustomerType)).Length);
        }while(customerType == oldCustomer);


        var ran = Mathf.Clamp(GameManager.Instance.currentDay, 0, System.Enum.GetValues(typeof(OrganType)).Length);
        desiredOrgan = (OrganType)Random.Range(0, ran);
        currentText = FindAnyObjectByType<HUDManager>().customerRequestUI.GetText(desiredOrgan);

        StartCoroutine(PlayParticles());
        isServed = false;

        FindAnyObjectByType<HUDManager>().customerRequestUI.gameObject.SetActive(false);
        //OnLookEnter();

        FindAnyObjectByType<VendorStand>().UpdateVenders();
    }

    public void SwitchCustomerModel()
    {
        switch (customerType)
        {
            case CustomerType.Zombie:
                transform.GetChild(2).GetChild(1).gameObject.SetActive(true);

                transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
                transform.GetChild(2).GetChild(2).gameObject.SetActive(false);
                break;
            case CustomerType.Skeleton:
                transform.GetChild(2).GetChild(2).gameObject.SetActive(true);

                transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
                transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
                break;
            case CustomerType.Witch:
                transform.GetChild(2).GetChild(0).gameObject.SetActive(true);

                transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
                transform.GetChild(2).GetChild(2).gameObject.SetActive(false);
                break;
        }
    }
    public void OnLookEnter()
    {
        print("test");
        customerRequest.enabled = true;
        isLookedAt = true;
        var customerUI = FindAnyObjectByType<HUDManager>().customerRequestUI;
        if (canBuyItem && !isServed)
        {
            // customerUI.gameObject.SetActive(true);
            // customerUI.SetText(currentText);

            List<OrganManager> organs = new List<OrganManager>();

            foreach (var organ in FindAnyObjectByType<VendorStand>().organsToSell)
            {
                if (organ.organType == desiredOrgan)
                {
                    organs.Add(organ);
                }
            }
            organs.Sort((a, b) => b.GetOrganPrice().CompareTo(a.GetOrganPrice()));

            customerUI.gameObject.SetActive(true);
            customerUI.SetTextSell(organs[0]);
            customerUI.SetIcon(desiredOrgan);

            RequestedOrgan = organs[0];

            //  FindAnyObjectByType<HUDManager>().UpdateCrossHairText($"Press E to sell {desiredOrgan.ToString()}");
            //  FindAnyObjectByType<HUDManager>().CrossHairText.transform.parent.parent.gameObject.SetActive(true);
        }
        else if (!canBuyItem && !isServed)
        {
            customerUI.gameObject.SetActive(true);
            customerUI.SetText(currentText);
            customerUI.SetIcon(desiredOrgan);
        }
    }
    public void OnLookExit()
    {
        if(customerRequest != null)
            customerRequest.enabled = false;
        isLookedAt = false;
        FindAnyObjectByType<HUDManager>().CrossHairText.transform.parent.parent.gameObject.SetActive(false);
        FindAnyObjectByType<HUDManager>().customerRequestUI.gameObject.SetActive(false);
        RequestedOrgan = null;
    }

    public IEnumerator PlayParticles()
    {
        blood.Play();
        yield return new WaitForSeconds(0.5f);
        SwitchCustomerModel();
        yield return new WaitForSeconds(0.5f);
        blood.Stop(false, ParticleSystemStopBehavior.StopEmitting);
    }
}

