using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CartMagnitism : MonoBehaviour
{
    public List<GameObject> CartObjects = new List<GameObject>();
    public CartBehavior cartBehavior;
    public GameObject objectHolder;
    public List<Rigidbody> rb = new List<Rigidbody>();

    private void Start()
    {
        cartBehavior = FindAnyObjectByType<CartBehavior>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //print(other.name);
        if (other.GetComponentInParent<OrganManager>() != null && !other.isTrigger)
        {
            var item = other.GetComponentInParent<OrganManager>().gameObject;
            CartObjects.Add(item);
            if (item.GetComponent<Rigidbody>() != null)
                rb.Add(item.GetComponent<Rigidbody>());
        }
        ObjectLock();
    }

    private void OnTriggerExit(Collider other)
    {
        CartObjects.Remove(other.gameObject);
        rb.Remove(other.gameObject.GetComponent<Rigidbody>());
        ObjectUnLock();
    }

    private void Update()
    {
        if (cartBehavior != null)
        {
       //     Debug.Log("no cartbehavior");
        }

    }
    public void ObjectLock()
    {
        foreach (GameObject obj in CartObjects)
        {
            obj.GetComponent<Transform>().SetParent(objectHolder.transform, true);
        }
        foreach (Rigidbody rb in rb)
        {
            rb.isKinematic = true;

        }
    }

    public void ObjectUnLock()
    {
        foreach (GameObject obj in CartObjects)
        {
            obj.GetComponent<Transform>().SetParent(null, true);
        }
        foreach (Rigidbody rb in rb)
        {
            rb.isKinematic = false;
        }
    }
}
