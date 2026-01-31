using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CartMagnitism : MonoBehaviour
{
    public List<GameObject> CartObjects = new List<GameObject>();
    public float Pull = 2.0f;
    CartBehavior cartBehavior;
    public GameObject objectHolder;

    private void Start()
    {
        cartBehavior = GetComponent<CartBehavior>();
    }

    private void OnTriggerEnter(Collider other)
    {
        CartObjects.Add(other.gameObject);
        ObjectLock();
    }

    private void OnTriggerExit(Collider other)
    {
        CartObjects.Remove(other.gameObject);
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
            //obj.GetComponent<Rigidbody>().enable = false;
            obj.GetComponent<Transform>().SetParent(objectHolder.transform);
        }
    }

    public void ObjectUnLock()
    {
        foreach (GameObject obj in CartObjects)
        {
            //obj.GetComponent<Rigidbody>().enable = false;
            obj.GetComponent<Transform>().SetParent(null);
        }
    }
}
