using System.Collections;
using TMPro;
using UnityEngine;

public class CartBehavior : MonoBehaviour
{
    public GameObject TriggerArea;
    public GameObject Player;
    public Transform cart_center;

    public bool moveing = false;
    public bool canPickUp = false;
    private void OnTriggerEnter(Collider other)
    {
      //  Debug.Log("collider");
        if (other.gameObject == Player && !moveing)
        {    
            canPickUp = true;

            // FollowPlayer(Player);
            // TriggerArea.SetActive(false);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //  Debug.Log("collider");
        if (other.gameObject == Player && !moveing)
        {
            canPickUp = false;

            // FollowPlayer(Player);
            // TriggerArea.SetActive(false);
        }
    }
    private void FollowPlayer(GameObject player)
    {
        TriggerArea.SetActive(false);
        this.transform.parent = cart_center;
        this.transform.position = cart_center.position;
        this.transform.rotation = cart_center.rotation;
        moveing = true;
    }

    private void Update()
    {


        if (moveing && Input.GetKeyDown(KeyCode.E) && !Player.GetComponent<HeldItem>().hasItem)
        {
           this.transform.parent = null;
           TriggerArea.SetActive(true);
           StartCoroutine(ResetMovement(1f));
        }
        if (canPickUp && Input.GetKeyDown(KeyCode.E) && !Player.GetComponent<HeldItem>().hasItem)
        {
            if(GameManager.Instance.currentDay == 0 && FindAnyObjectByType<FirstDayManager>().currentScreen == 5)
            {
                var fmd = FindAnyObjectByType<FirstDayManager>();
                fmd.currentScreen++;
                fmd.isShowingScreen = true;
                fmd.tutorialScreens[fmd.currentScreen].SetActive(true);
                StartCoroutine(fmd.WaitForNextScreen());
            }
            FollowPlayer(Player);
            canPickUp = false;

        }
    }

    IEnumerator ResetMovement(float time)
    {
        yield return new WaitForSeconds(time);
        moveing = false;
    }
}
