using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

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
        print(other.name);
        if (other.GetComponentInParent<OrganManager>() == null || other.isTrigger || CartObjects.Contains(other.GetComponentInParent<OrganManager>().gameObject))
            return;


        var item = other.GetComponentInParent<OrganManager>().gameObject;
        CartObjects.Add(item);
        if (item.GetComponent<Rigidbody>() != null)
        rb.Add(item.GetComponent<Rigidbody>());
      //  item.GetComponent<Rigidbody>().MovePosition(item.GetComponent<Rigidbody>().position + Vector3.down * 0.05f);
        //  item.GetComponent<Rigidbody>().excludeLayers = LayerMask.GetMask("OrganIgnore", "Organs");
        ObjectLock(item, item.GetComponent<Rigidbody>());
        
    }

    private void OnTriggerExit(Collider other)
    {
        

        if (!CartObjects.Contains(other.GetComponentInParent<OrganManager>().gameObject))
            return;

        var item = other.GetComponentInParent<OrganManager>().gameObject;
        CartObjects.Remove(item);
        rb.Remove(item.GetComponent<Rigidbody>());
      //  other.gameObject.GetComponent<Rigidbody>().excludeLayers = LayerMask.GetMask(, "Organs");
        ObjectUnLock(item, item.GetComponent<Rigidbody>());
    }

    private void Update()
    {
        if (cartBehavior != null)
        {
       //     Debug.Log("no cartbehavior");
        }

    }
    public void ObjectLock(GameObject obj, Rigidbody rb)
    {

            obj.GetComponent<Transform>().SetParent(objectHolder.transform, true);
            rb.GetComponent<OrganManager>().toolTip.enabled = false;
            rb.isKinematic = true;
    }

    public void ObjectUnLock(GameObject obj, Rigidbody rb)
    {
        
            obj.GetComponent<Transform>().SetParent(null, true);
            rb.isKinematic = false;
    }
}
