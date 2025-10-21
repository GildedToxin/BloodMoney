using TMPro;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;
using UnityEngine.VFX;

public enum OrganType
{
    Brain,
    Bone,
    Meat,
    Limb,
    Fingers,
    Eyes,
    Blood
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

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            LaunchForward(GetComponent<Rigidbody>());
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        float impactSpeed = collision.relativeVelocity.magnitude;

        if(currentHealth <= 0)
            return; // Organ is already destroyed

        currentHealth -= Mathf.RoundToInt(Mathf.Min(impactSpeed, maxDamage));
        qualityText.GetComponent<TextMeshProUGUI>().text = $"Quality: {currentHealth}%";
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Organ destroyed!");
            Destroy(gameObject);
        }

        int index = Random.Range(0, decalMaterials.Length);
        Material chosenMaterial = decalMaterials[index];


        Vector3 avgPoint = Vector3.zero;
        foreach (var c in collision.contacts)
            avgPoint += c.point;

        avgPoint /= collision.contacts.Length;


        GameObject decal = Instantiate(decalPrefab, avgPoint, Quaternion.LookRotation(-collision.contacts[0].normal));


        decal.transform.SetParent(collision.transform);
        decal.transform.Rotate(Vector3.forward, Random.Range(0f, 360f));
        decal.GetComponent<DecalProjector>().material = chosenMaterial;

        GameObject vfxInstance = Instantiate(bloodEffect, transform.position, Quaternion.LookRotation(-collision.contacts[0].normal));
        Destroy(vfxInstance.gameObject, 15);
    }

    public void OnLookEnter()
    {
        toolTip.enabled = true;
    }
    public void OnLookExit()
    {
        toolTip.enabled = false;
    }

   

    public void LaunchForward(Rigidbody rb)
    {
        if (rb == null) return;

        // Reset velocity first (optional)
        rb.linearVelocity = Vector3.zero;

        // Apply force in the object's forward direction
        rb.AddForce(transform.forward * 50, ForceMode.VelocityChange);
    }

    public int GetOrganPrice()  
    {

        return 1;
    }

    public void RefreshOrgan()
    {
        Item myAsset = Resources.Load<Item>($"items/{organType.ToString()}");

    }
}
