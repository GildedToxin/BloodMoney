using UnityEngine;

public class LockYPos : MonoBehaviour
{
    public Transform player;   // Assign your Player object here

    private float startYOffset;
    private float startXOffset;

    void Start()
    {
        player = FindAnyObjectByType<PlayerController>().transform;
        // Store the starting world-space offset in Y between this point and the player
        startYOffset = transform.position.y - player.position.y;
        startXOffset = transform.position.x - player.position.x;
    }

    void Update()
    {
        // Keep the same Y offset relative to the player
        Vector3 pos = transform.position;
        pos.y = player.position.y + startYOffset;
        transform.position = pos;

      
    }
}
