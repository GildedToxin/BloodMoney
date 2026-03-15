using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;

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
                FindAnyObjectByType<VendorStand>().SellOrgan(desiredOrgan, this);
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

        desiredOrgan = (OrganType)Random.Range(0, System.Enum.GetValues(typeof(OrganType)).Length);

        StartCoroutine(PlayParticles());
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
        customerRequest.enabled = true;
        isLookedAt = true;

        if (canBuyItem && !isServed)
        {
            FindAnyObjectByType<HUDManager>().UpdateCrossHairText($"Press E to sell {desiredOrgan.ToString()}");
            FindAnyObjectByType<HUDManager>().CrossHairText.transform.parent.parent.gameObject.SetActive(true);
        }
    }
    public void OnLookExit()
    {
        if(customerRequest != null)
            customerRequest.enabled = false;
        isLookedAt = false;
        FindAnyObjectByType<HUDManager>().CrossHairText.transform.parent.parent.gameObject.SetActive(false);
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

