using UnityEngine;

[ExecuteAlways]
public class CameraPlayerFolower : MonoBehaviour
{
    public Transform cameraPosition;

    private void Update()
    {
        transform.position = cameraPosition.position;
    }
}
