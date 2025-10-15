using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CartItemMagnitism : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> organs = new List<GameObject>();
    public CartScript cartScript;

    public float maxDistance;
    public float maxStrength;

    void Update()
    {
        if (cartScript.isMoving)
        {
            updateMagnetism();
        }
    }

    void updateMagnetism()
    {
        for (int i = 0; i < organs.Count; i++) 
        {
            Rigidbody organRB = organs[i].GetComponent<Rigidbody>();
            Collider organC = organs[i].GetComponent<Collider>();

            Physics.IgnoreCollision(organC, organs[i].GetComponent<Collider>());
            //organRB.constraints = RigidbodyConstraints.FreezePositionY;
            float distance = Vector3.Distance(organRB.transform.position, this.transform.position); //distance between item and cart

            if (distance < maxDistance)
            {
                float TotalDistance = Mathf.InverseLerp(maxDistance, 0f, distance); //total distance between item and cart
                float strength = Mathf.Lerp(0f, maxStrength, TotalDistance); //strength of the magnitism
                Vector3 cartDirection = (this.transform.position - organRB.transform.position).normalized; // direction of the cart reletive to the player

                organRB.AddForce(cartDirection * strength, ForceMode.Force); //applies magnitism
            }
        }
    }
}
