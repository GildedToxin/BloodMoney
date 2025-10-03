using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrganManager : MonoBehaviour, IPlayerLookTarget
{
    public int maxHealth = 100;
    public int currentHealth = 100;
    public int maxDamage = 50;
    public Canvas toolTip;
    public GameObject qualityText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
    }

    public void OnLookEnter()
    {
        toolTip.enabled = true;
    }
    public void OnLookExit()
    {
        toolTip.enabled = false;
    }
}
