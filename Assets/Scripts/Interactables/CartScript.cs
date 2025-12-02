using UnityEngine;

public class CartScript : MonoBehaviour, IInteractable
{
    [Header("Cart Mechanics")]
    public bool isMoving = false;
    public GameObject cartHolder;
    public Interact interact;
    public PlayerController playerController;
    public GameObject player;

    [Header("Keybinds")]
    public KeyCode letGoKey = KeyCode.T;

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
            player.layer = LayerMask.NameToLayer("TransparentFX");
            playerController.canJump = false;
            transform.position = cartHolder.transform.position;
            transform.rotation = cartHolder.transform.rotation;
            if (Input.GetKey(letGoKey) && isMoving)
            {
                player.layer = LayerMask.NameToLayer("Default");
                playerController.canJump = true;
                isMoving = false;
                transform.position = this.transform.position;
                transform.rotation = this.transform.rotation;
                interact.noInteraction = true;
            }
        }
    }
}
