using System.Collections;
using TMPro;
using UnityEngine;

public class CartBehavior : MonoBehaviour
{
    public GameObject TriggerArea;
    public GameObject Player;
    public Transform cart_center;

    private bool moveing = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player && !moveing)
        {
            FollowPlayer(Player);
            TriggerArea.SetActive(false);
        }
    }

    private void FollowPlayer(GameObject player)
    {
        this.transform.parent = player.transform;
        this.transform.position = cart_center.position;
        moveing = true;
    }

    private void Update()
    {
        if (moveing)
        {
            if(Input.GetKeyDown(KeyCode.RightShift))
            {
                this.transform.parent = null;
                TriggerArea.SetActive(true);
                StartCoroutine(ResetMovement(1f));
            }
        }
    }

    IEnumerator ResetMovement(float time)
    {
        yield return new WaitForSeconds(time);
        moveing = false;
    }
}
