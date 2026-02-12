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
        if (other.GetComponent<OrganManager>() != null)
        {
            CartObjects.Add(other.gameObject);
            if (other.gameObject.GetComponent<Rigidbody>() != null)
                rb.Add(other.gameObject.GetComponent<Rigidbody>());
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
            Debug.Log("no cartbehavior");
        }

    }
    public void ObjectLock()
    {
        foreach (GameObject obj in CartObjects)
        {
            obj.GetComponent<Transform>().SetParent(objectHolder.transform);
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
            obj.GetComponent<Transform>().SetParent(null);
        }
        foreach (Rigidbody rb in rb)
        {
            rb.isKinematic = false;
        }
    }
}
