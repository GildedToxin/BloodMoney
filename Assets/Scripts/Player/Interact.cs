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
    void Update()
    {
        // Uses a Raycast to find an object and checks if its interactable, then runs its interaction code
        if (Input.GetKey(interactKey))
        {
            Ray r = new Ray(interactionSource.position, interactionSource.forward);
            if (Physics.Raycast(r, out RaycastHit hitInfo, interactionRange))
            {
                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                {
                    interactObj.Interact();
                }
            }
        }
    }
}
