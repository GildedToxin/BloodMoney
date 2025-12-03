using UnityEngine;

[ExecuteAlways]
public class CameraPlayerFolower : MonoBehaviour
{
    public Transform cameraPosition;

    private void LateUpdate()
    {
        transform.position = cameraPosition.position;
    }
}
