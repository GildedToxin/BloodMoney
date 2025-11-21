using UnityEngine;
using System.Collections.Generic;

public class HeldItem : MonoBehaviour
{
    //Place script onto player and call functions from the inventory scripts

    //public List<GameObject> itemPrefabs; // List of possible item prefabs to choose from
    public GameObject currentItem;
    public Transform heldItemPosition; // Drag and drop the position in the inspector or set it manually
    public bool hasItem = false;
    public float lerpSpeed = 5f;

    public Animator RightHand;

    private void Update()
    {
        if(hasItem && currentItem != null)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                DropItem(currentItem);
                return;
            }
            var rb = currentItem.GetComponent<Rigidbody>();
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, Time.deltaTime * lerpSpeed);
            rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, Vector3.zero, Time.deltaTime * lerpSpeed);
            currentItem.transform.position = heldItemPosition.position;
        }

        RightHand.SetBool("isGrabbing", hasItem);
        Debug.Log("is this thing on???");
    }
    public void PickUpItem(GameObject item)
    {
        hasItem = true;
        currentItem = item;
        currentItem.transform.position = heldItemPosition.position;
        currentItem.transform.SetParent(heldItemPosition);
        currentItem.GetComponent<Rigidbody>().useGravity = false;

    }
    public void DropItem(GameObject item)
    {
        hasItem = false;
        currentItem.transform.SetParent(null);
        var rb = currentItem.GetComponent<Rigidbody>();
        rb.useGravity = true;
        currentItem = null;

    }
}
