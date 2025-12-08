using UnityEngine;

[ExecuteAlways]
public class CameraPlayerFolower : MonoBehaviour
{
    public Transform cameraPosition;
    public CartScript cartScript;
    private float yPosition;

    private void Awake()
    {
        yPosition = cameraPosition.transform.position.y;
    }

    private void LateUpdate()
    {
        if (cartScript.isMoving)
        {
            transform.position = new Vector3(cameraPosition.transform.position.x, yPosition, cameraPosition.transform.position.z);
        }
        else
        {
            transform.position = cameraPosition.position;
        }
    }
}
