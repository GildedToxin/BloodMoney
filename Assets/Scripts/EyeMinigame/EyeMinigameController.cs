using Unity.Hierarchy;
using UnityEngine;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class EyeMinigameController : MonoBehaviour
{
    public int maxY;
    public int minY;
    public int maxX;
    public int minX;
    public float movementSpeed = 1f;


    public GameObject scoop;
    private bool miniGameRunning = true;
    private int horizontalMovement = 0;


    public void Update()
    {
        if (miniGameRunning)
        {
            if (horizontalMovement == 0)
            {
                float y = Mathf.PingPong(Time.time * movementSpeed, 1) * maxY - minY;
                scoop.transform.position = new Vector3(scoop.transform.position.x, y, scoop.transform.position.z);
            }
            else if (horizontalMovement == 1)
            {
                float x = Mathf.PingPong(Time.time * movementSpeed, 1) * maxX - minX;
                scoop.transform.position = new Vector3(x, scoop.transform.position.y, scoop.transform.position.z);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && horizontalMovement == 0)
        {
            scoop.transform.position = new Vector3(scoop.transform.position.x, scoop.transform.position.y, scoop.transform.position.z);
            horizontalMovement = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && horizontalMovement == 1)
        {
            Debug.Log("CheckSpot");
        }
    }
}
