using UnityEngine;

public enum CustomerType
{
    Werewolf,
    Vampire,
    Zombie,
    Skeleton,
    Whitch,
    Frankestein
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isLookedAt && canBuyItem && !isServed)
        {
            if (Input.GetKeyDown(KeyCode.Q)) {
                isServed = true;
                FindAnyObjectByType<VendorStand>().SellOrgan(desiredOrgan, this);
            }

           
        } 
    }

    public void RandomCustomer()
    {
        customerType = (CustomerType)Random.Range(0, System.Enum.GetValues(typeof(CustomerType)).Length);
    }


    public void OnLookEnter()
    {
        customerRequest.enabled = true;
        isLookedAt = true;

        if (canBuyItem && !isServed)
        {
            FindAnyObjectByType<HUDManager>().UpdateSellAsk(desiredOrgan.ToString());
            FindAnyObjectByType<HUDManager>().sellItem.SetActive(true);
        }
    }
    public void OnLookExit()
    {
        if(customerRequest != null)
            customerRequest.enabled = false;
        isLookedAt = false;
        FindAnyObjectByType<HUDManager>().sellItem.SetActive(false);
    }

}

