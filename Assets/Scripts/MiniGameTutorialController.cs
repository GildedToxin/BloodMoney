using UnityEngine;

public class MiniGameTutorialController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.gameObject.SetActive(true); 
        Cursor.lockState = CursorLockMode.Confined; // Confine the cursor to the game window
        Cursor.visible = true; // Show the cursor when the tutorial is active

    }

    // Update is called once per frame
    void Update()
    {
       // Cursor.visible = true;
    }

    public void EndTutorial()
    {
        this.transform.parent.gameObject.SetActive(false); 
    }
}
