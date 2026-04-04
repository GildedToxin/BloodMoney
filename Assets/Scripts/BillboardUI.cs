using UnityEngine;

public class BillboardUI : MonoBehaviour

{
    public Transform target;
    public float heightOffset = 2f;

    void LateUpdate()
    {
        // Always stay above the object's CENTER in world space
        transform.position = target.position + Vector3.up * heightOffset;

        transform.rotation = Quaternion.identity;
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0); // Fix flipping if needed
    }
}
