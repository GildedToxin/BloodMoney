using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float sensitivityX;
    public float sensitivityY;

    public Transform orientation;

    private float xRotation;
    private float yRotation;

    [SerializeField] private int maxRotation = 90;
    [SerializeField] private int minRotation = -90;

    public bool isUIOpen;

    public CartScript cartScript;
    void Start()
    {
        //Keeps Cursor in place and invisible for gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void FixedUpdate()
    {
        if (cartScript != null && cartScript.isMoving)
            sensitivityY = 0;
        else
            sensitivityY = 400;


        if (isUIOpen)
            return;

        //Mouse Camera Inputs
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivityX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivityY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minRotation, maxRotation);

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
