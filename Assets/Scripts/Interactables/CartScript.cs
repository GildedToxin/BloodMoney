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
    public GameObject PlayerPosition;

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


            var cleaningCart = transform.parent;

 
            Transform parent = cleaningCart.parent;


            cleaningCart.localPosition = Vector3.zero;


            Vector3 local = cleaningCart.localEulerAngles;
            cleaningCart.localEulerAngles = new Vector3(0, 0, local.z);


            Vector3 world = cleaningCart.rotation.eulerAngles;
            world.z = 0;
            cleaningCart.rotation = Quaternion.Euler(world);

            local = cleaningCart.localEulerAngles;
            cleaningCart.localEulerAngles = new Vector3(0, 0, local.z);



            if (Input.GetKey(letGoKey) && isMoving)
            {
                player.layer = LayerMask.NameToLayer("Default");
                playerController.canJump = true;
                isMoving = false;
                transform.parent.parent = null;
                interact.noInteraction = true;
            }
        }
    }
}
