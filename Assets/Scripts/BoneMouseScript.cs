using UnityEngine;

public class BoneMouseScript : MonoBehaviour
{
    public RectTransform cursorImage;

    void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        cursorImage.position = mousePos;
    }

    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
        cursorImage.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        cursorImage.gameObject.SetActive(true);
    }
}