using UnityEngine;

public class OrganPopUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerController>())
            GetComponentInParent<OrganManager>().toolTip.gameObject.SetActive(true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>())
            GetComponentInParent<OrganManager>().toolTip.gameObject.SetActive(false);
    }
}
