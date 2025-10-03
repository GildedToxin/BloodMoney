using UnityEngine;

public class CartScript : MonoBehaviour, IInteractable
{
    public bool isMoving = false;
    public GameObject player;
    public void Interact()
    {
        if (isMoving)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }
    }

    public void Update()
    {
        if (isMoving)
        {
            transform.position = player.transform.position;
            transform.rotation = player.transform.rotation;
        }
    }
}
