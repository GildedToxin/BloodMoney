using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;
using UnityEngine.VFX;

public enum OrganType
{
    Bone,
    Limb,
    Blood,
    Brain,
    Hand,
    Eye
}

public class OrganManager : MonoBehaviour, IPlayerLookTarget
{
    public int maxHealth = 100;
    public int currentHealth = 100;
    public int maxDamage = 50;
    public Canvas toolTip;
    public GameObject qualityText;
    public OrganType organType;


    public GameObject decalPrefab;
    public Material[] decalMaterials;
    public float decalOffset = 0.01f;
    public GameObject bloodEffect;

    public Item itemData;

    public bool islookedAt = false;

    public Vector3 lockedPosition;
    public Quaternion lockedRotation;

    public AudioClip breakSFX;

    public bool canTakeDamage = false;
    void Start()
    {
        itemData = Resources.Load<Item>($"items/{organType.ToString()}");
        StartCoroutine(TakeDamage());
    }
    void Update()
    {
        if(islookedAt && Input.GetKeyDown(KeyCode.E) && !FindAnyObjectByType<HeldItem>().hasItem && !FindAnyObjectByType<CartBehavior>().moveing)
        {
            FindAnyObjectByType<HeldItem>().PickUpItem(gameObject);
            toolTip.enabled = false;
        }

        if (!FindAnyObjectByType<CartMagnitism>().CartObjects.Contains(this.gameObject))
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
    void LateUpdate()
    {
        if (GetComponent<Rigidbody>().isKinematic && FindAnyObjectByType<CartBehavior>().moveing){

            transform.localPosition = lockedPosition;
            transform.localRotation = lockedRotation;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if(canTakeDamage == false)
            return;

        float impactSpeed = collision.relativeVelocity.magnitude;

        if(currentHealth <= 0)
            return; // Organ is already destroyed

        currentHealth -= Mathf.RoundToInt(Mathf.Min(impactSpeed, maxDamage));
        toolTip.transform.GetChild(0).GetChild(1).GetComponent<Image>().fillAmount = (float)currentHealth / maxHealth;
        qualityText.GetComponent<TextMeshProUGUI>().text = currentHealth.ToString();
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Organ destroyed!");
            if(FindAnyObjectByType<VendorStand>().organsToSell.Contains(this))
            {
                FindAnyObjectByType<VendorStand>().organsToSell.Remove(this);
                FindAnyObjectByType<VendorStand>().UpdateVenders();
            }
            Destroy(gameObject);
            AudioPool.Instance.PlayClip2D(breakSFX, volume: 0.5f, pitch: 1.1f);
        }

    }
 
    
    public void OnLookEnter()
    {
       
        //toolTip.enabled = true;

        if(FindFirstObjectByType<CartBehavior>() != null && FindFirstObjectByType<CartBehavior>().moveing || FindFirstObjectByType<CartBehavior>().canPickUp)
             return;
        islookedAt = true;
        FindFirstObjectByType<HUDManager>().UpdateCrossHairText("Press E to pick up");
        FindFirstObjectByType<HUDManager>().CrossHairText.transform.parent.parent.gameObject.SetActive(true);
    }
    public void OnLookExit()
    {
        
        //toolTip.enabled = false;

        if (FindFirstObjectByType<CartBehavior>() != null && FindFirstObjectByType<CartBehavior>().moveing || FindFirstObjectByType<CartBehavior>().canPickUp)
            return;
        islookedAt = false;
        FindFirstObjectByType<HUDManager>().CrossHairText.transform.parent.parent.gameObject.SetActive(false);
    }

    public IEnumerator TakeDamage()
    {
        yield return new WaitForSeconds(2f);
        canTakeDamage = true;
    }


    [ContextMenu("Get Organ Price")]
    public int GetOrganPrice()  
    {
        float mult = (currentHealth / 100f);
        return (int)(itemData.price * mult); //+  10 *  mult;
    }

    public void RefreshOrgan()
    {
        Item myAsset = Resources.Load<Item>($"items/{organType.ToString()}");

    }
}
