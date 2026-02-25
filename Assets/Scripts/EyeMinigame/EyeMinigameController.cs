using Unity.Hierarchy;
using UnityEngine;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class EyeMinigameController : MonoBehaviour
{
    public float maxY = 0f;
    public float minY = 0f;
    public float maxX = 0f;
    public float minX = 0f;

    public float movementSpeed = 0.1f;

    public GameObject scoop;
    private bool miniGameRunning = true;
    private int direction = 0;


    public void Update()
    {
        if (miniGameRunning)
        {
            Debug.Log("isRunning");
            if (direction == 0)
            {
                Debug.Log("goingUp");
                float moveUp = scoop.transform.position.y + movementSpeed * Time.deltaTime;
                transform.position = new Vector3(scoop.transform.position.x, moveUp, scoop.transform.position.z);
                if (scoop.transform.position.y == maxY)
                {
                    direction = 1;
                }
            }
            else if (direction == 1)
            {
                Debug.Log("goingDown");
                float moveDown = scoop.transform.position.y - movementSpeed * Time.deltaTime;
                transform.position = new Vector3(scoop.transform.position.x, moveDown, scoop.transform.position.z);
                if (scoop.transform.position.y == minY)
                {
                    direction = 0;
                }
            }
        }
    }
}
