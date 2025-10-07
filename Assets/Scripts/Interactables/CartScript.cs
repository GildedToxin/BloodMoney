using UnityEngine;

public class CartScript : MonoBehaviour, IInteractable
{
    [Header("Cart Mechanics")]
    public bool isMoving = false;
    public GameObject player;
    public Interact interact;

    [Header("Keybinds")]
    public KeyCode letGoKey = KeyCode.T;

    public void Awake()
    {
        //interact = GetComponent<Interact>();// find a way to automaticaly assign this
    }
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
            if (Input.GetKey(letGoKey) && isMoving)
            {
                isMoving = false;
                transform.position = this.transform.position;
                transform.rotation = this.transform.rotation;
                interact.noInteraction = true;
            }
        }
    }
}
