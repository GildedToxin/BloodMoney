using UnityEngine;

interface IInteractable{
    public void Interact();
}

public class Interact : MonoBehaviour {

    [Header("Keybinds")]
    public KeyCode interactKey = KeyCode.E;

    [Header("Interaction")]
    public Transform interactionSource;
    public float interactionRange;
    public bool noInteraction = true;
    void Update()
    {
        // Uses a Raycast to find an object and checks if its interactable, then runs its interaction code
        if (Input.GetKey(interactKey) && noInteraction)
        {
            Ray r = new Ray(interactionSource.position, transform.forward);
            Debug.DrawRay(interactionSource.position, interactionSource.forward, Color.red);    
            if (Physics.Raycast(r, out RaycastHit hitInfo, interactionRange))
            {
                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                {
                    noInteraction = false;
                    interactObj.Interact();
                }
            }
        }
    }
}
