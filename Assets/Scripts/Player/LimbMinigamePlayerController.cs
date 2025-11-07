using UnityEngine;

public class LimbMinigamePlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject playerLimb;
    private float moveSpeed = 30f;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.position += Vector3.left * 0.1f * Time.deltaTime * (moveSpeed * 0.25f);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.position += Vector3.right * 0.1f * Time.deltaTime * (moveSpeed * 0.25f);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            playerLimb.transform.Rotate(Vector3.up, -1f * Time.deltaTime * moveSpeed);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            playerLimb.transform.Rotate(Vector3.up, 1f * Time.deltaTime * moveSpeed);
        }
    }
}
