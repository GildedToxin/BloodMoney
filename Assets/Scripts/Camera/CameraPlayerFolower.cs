using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

[ExecuteAlways]
public class CameraPlayerFolower : MonoBehaviour
{
    public Transform cameraPosition;

    private void LateUpdate()
    {

        transform.position = cameraPosition.position;
    }
}
