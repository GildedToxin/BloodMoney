using UnityEngine;

public class CartScript : MonoBehaviour, IInteractable
{
    [Header("Cart Mechanics")]
    public bool isMoving = false;
    public GameObject cartHolder;
    public GameObject cart;
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
            transform.parent.parent = cartHolder.transform;
            //player.transform.position = cartHolder.transform.position;
            if (Input.GetKey(letGoKey) && isMoving)
            {
                player.layer = LayerMask.NameToLayer("Default");
                playerController.canJump = true;
                isMoving = false;
                transform.parent = null;
                interact.noInteraction = true;
            }
        }
    }
}
