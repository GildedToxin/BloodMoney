using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    private Vector3 position;
    public float offset = 3f;

    Ray ray;
    RaycastHit hit;
    public bool follow = false;

    public GameObject follower;

    private void Update()
    {
        position = Input.mousePosition;
        ray = Camera.main.ScreenPointToRay(position);
        position.z = offset;

        if(Physics.Raycast(ray, out hit) && hit.collider.name == "Mouse_Follower")
        {
            follow = true;
        }

        if (follow == true)
        {
            transform.position = Camera.main.ScreenToWorldPoint(position);
            follower.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
    }
}
