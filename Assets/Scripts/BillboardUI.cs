using UnityEngine;

public class BillboardUI : MonoBehaviour
{   void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0); // Fix flipping if needed
    }
}
