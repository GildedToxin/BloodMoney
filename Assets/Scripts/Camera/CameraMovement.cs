using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float sensitivityX;
    public float sensitivityY;

    public Transform orientation;

    private float xRotation;
    private float yRotation;

    public bool isUIOpen;
    void Start()
    {
        //Keeps Cursor in place and invisible for gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void FixedUpdate()
    {
        if (isUIOpen)
            return;

        //Mouse Camera Inputs
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivityX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivityY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        // Rotates Camera and Orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void OpenUI()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        isUIOpen = true;
    }
    public void CloseUI()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isUIOpen = false;
    }
}
