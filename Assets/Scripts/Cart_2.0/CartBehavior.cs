using System.Collections;
using TMPro;
using UnityEngine;

public class CartBehavior : MonoBehaviour, IPlayerLookTarget
{
    public GameObject TriggerArea;
    public GameObject Player;
    public Transform cart_center;

    public bool moveing = false;
    public bool canPickUp = false;
    public bool isLookedAt = false;
    public void OnLookEnter()
    {
        isLookedAt = true;

        if (moveing)
        {
            FindFirstObjectByType<HUDManager>().UpdateCrossHairText("Press E to drop cart");
            FindFirstObjectByType<HUDManager>().CrossHairText.transform.parent.parent.gameObject.SetActive(true);

        }
    }
    public void OnLookExit()
    {
        isLookedAt = false;

        if (moveing)
            FindFirstObjectByType<HUDManager>().CrossHairText.transform.parent.parent.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
      //  Debug.Log("collider");
        if (other.gameObject == Player && !moveing)
        {
            FindFirstObjectByType<HUDManager>().UpdateCrossHairText("Press E to push cart");
            FindFirstObjectByType<HUDManager>().CrossHairText.transform.parent.parent.gameObject.SetActive(true);

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
            FindFirstObjectByType<HUDManager>().CrossHairText.transform.parent.parent.gameObject.SetActive(false);
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
        if(moveing == false && canPickUp == false)
        {
        //    this.transform.parent = null;
        //    TriggerArea.SetActive(true);
        //    StartCoroutine(ResetMovement(1f));


        }
        if (this.transform.parent != null && moveing == false)
            moveing = true;


        if (isLookedAt && moveing && Input.GetKeyDown(KeyCode.E) && !Player.GetComponent<HeldItem>().hasItem && (FindFirstObjectByType<FirstDayManager>() == null || !FindFirstObjectByType<FirstDayManager>().isShowingScreen))
        {
            FindFirstObjectByType<HUDManager>().CrossHairText.transform.parent.parent.gameObject.SetActive(false);
            this.transform.parent = null;
            TriggerArea.SetActive(true);
            StartCoroutine(ResetMovement(1f));
        }
        if (canPickUp && Input.GetKeyDown(KeyCode.E) && !Player.GetComponent<HeldItem>().hasItem && (FindFirstObjectByType<FirstDayManager>() == null || !FindFirstObjectByType<FirstDayManager>().isShowingScreen))
        {
            if(GameManager.Instance.currentDay == 0 && FindAnyObjectByType<FirstDayManager>().currentScreen == 5)
            {
                var fdm = FindAnyObjectByType<FirstDayManager>();
                fdm.currentScreen++;
                fdm.isShowingScreen = true;
                fdm.tutorialScreens[fdm.currentScreen].SetActive(true);
                StartCoroutine(fdm.WaitForNextScreen());
            }
            FollowPlayer(Player);
            canPickUp = false;

            FindFirstObjectByType<HUDManager>().UpdateCrossHairText("");
            FindFirstObjectByType<HUDManager>().CrossHairText.transform.parent.parent.gameObject.SetActive(false);

        }
    }

    IEnumerator ResetMovement(float time)
    {
        yield return new WaitForSeconds(time);
        moveing = false;
    }
}
