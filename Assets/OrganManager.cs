using UnityEngine;

public class OrganManager : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth = 100;
    public int maxDamage = 50;
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

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Organ destroyed!");
            Destroy(gameObject);
        }
    }
}
