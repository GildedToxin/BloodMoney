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
    void SetupRigidbody(GameObject obj)
    {
        Rigidbody rb = obj.AddComponent<Rigidbody>();

        rb.mass = 1f;
        rb.linearDamping = 0f;
        rb.angularDamping = 0.05f;
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.None;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }
    private void FollowPlayer(GameObject player)
    {
        //Destroy(GetComponent<Rigidbody>());
        TriggerArea.SetActive(false);
        this.transform.parent = cart_center;
        this.transform.position = cart_center.position;
        this.transform.rotation = cart_center.rotation;
        moveing = true;

        foreach(Rigidbody rb in GetComponentInChildren<CartMagnitism>().rb)
        {
            if (rb == null)
                continue;
            rb.GetComponent<OrganManager>().lockedPosition = rb.transform.localPosition;
            rb.GetComponent<OrganManager>().lockedRotation = rb.transform.localRotation;
        }
    }

    private void Update()
    {
        if(moveing == false && canPickUp == false)
        {
        //    this.transform.parent = null;
        //    TriggerArea.SetActive(true);
        //    StartCoroutine(ResetMovement(1f));
        //    StartCoroutine(ResetMovement(1f));


        }
        if (this.transform.parent != null && moveing == false)
            moveing = true;

        if (moveing && Camera.main.transform.eulerAngles.x >= 30 && Camera.main.transform.eulerAngles.x <= 80 )
        {
           // FindFirstObjectByType<HUDManager>().UpdateCrossHairText("Press E to drop cart");
       //     FindFirstObjectByType<HUDManager>().CrossHairText.transform.parent.parent.gameObject.SetActive(true);

        }

        if (!GameManager.Instance.isInMiniGame && Camera.main.transform.eulerAngles.x >= 30 && Camera.main.transform.eulerAngles.x <= 80 && moveing && !Player.GetComponent<HeldItem>().hasItem)
        {
            FindFirstObjectByType<HUDManager>().UpdateCrossHairText("Press E to drop cart");
            FindFirstObjectByType<HUDManager>().CrossHairText.transform.parent.parent.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                FindFirstObjectByType<HUDManager>().CrossHairText.transform.parent.parent.gameObject.SetActive(false);
                moveing= false;
                this.transform.parent = null;
                TriggerArea.SetActive(true);
                StartCoroutine(ResetMovement(1f));
                //SetupRigidbody(this.gameObject);
            }
        }
        else if (moveing)
        {
           // FindFirstObjectByType<HUDManager>().CrossHairText.transform.parent.parent.gameObject.SetActive(false);
        }
        else
        {
            //print(Camera.main.transform.eulerAngles.x);
        }
        if (isLookedAt && canPickUp && !Player.GetComponent<HeldItem>().hasItem)
        {
            FindFirstObjectByType<HUDManager>().UpdateCrossHairText("Press E to push cart");
            FindFirstObjectByType<HUDManager>().CrossHairText.transform.parent.parent.gameObject.SetActive(true);
        }
        if (isLookedAt && canPickUp && Input.GetKeyDown(KeyCode.E) && !Player.GetComponent<HeldItem>().hasItem)
        {
   
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
