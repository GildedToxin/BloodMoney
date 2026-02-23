using UnityEngine;
using System.Collections;
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
    public Animator LeftHand;

    public bool canDropItem;

    public bool hasShownScreen = false;
    private void Update()
    {
        if(hasItem && currentItem != null)
        {
            if(canDropItem && Input.GetKeyDown(KeyCode.E) && (FindFirstObjectByType<MeatGrinder>() == null || !FindFirstObjectByType<MeatGrinder>().isLookedAt))
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
        LeftHand.SetBool("isGrabbing", hasItem);
        //Debug.Log("is this thing on???");
    }
    public void PickUpItem(GameObject item)
    {
        if (GameManager.Instance.currentDay == 0 && !hasShownScreen && FindAnyObjectByType<FirstDayManager>().currentScreen == 11)
        {
            var fdm = FindAnyObjectByType<FirstDayManager>();
            fdm.currentScreen++; // 12
            fdm.isShowingScreen = true;
            fdm.tutorialScreens[fdm.currentScreen].SetActive(true);
            hasShownScreen = true;
            StartCoroutine(fdm.WaitForNextScreen());
        }

        StartCoroutine(DropCooldownTimer());
        hasItem = true;
        currentItem = item;
        currentItem.transform.position = heldItemPosition.position;
        currentItem.transform.SetParent(heldItemPosition);
        currentItem.GetComponent<Rigidbody>().useGravity = false;
        GetComponent<InventoryController>().HideAllTools();
        currentItem.GetComponent<Rigidbody>().isKinematic = false;

    }
    public void DropItem(GameObject item)
    {
        StartCoroutine(PickUpCooldownTimer());
        currentItem.transform.SetParent(null, worldPositionStays: true);
        var rb = currentItem.GetComponent<Rigidbody>();
        rb.useGravity = true;
        currentItem = null;
        canDropItem = false;
        GetComponent<InventoryController>().ChangeModel(GetComponent<InventoryController>().selectedIndex.Value);
    }
    IEnumerator PickUpCooldownTimer()
    {

        yield return new WaitForSeconds(0.5f);
        hasItem = false;
    }
    IEnumerator DropCooldownTimer()
    {

        yield return new WaitForSeconds(0.5f);
        canDropItem = true;
    }
}
