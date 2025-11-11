using UnityEngine;

public class LimbMinigamePlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject playerLimb;
    private float moveSpeed = 30f;

    [SerializeField]
    private float collectedPoints = 0f;
    private float scorePercentage = 0f;
    private float distanceMoved = 0f;
    private float startingNumberOfPoints = 0f;
    private bool endMinigame = false;

    void Start()
    {
        startingNumberOfPoints = playerLimb.GetComponentInChildren<LimbCuttingScript>().numberOfPoints;
    }

    void Update()
    {
        #region Player Input
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
            playerLimb.transform.Rotate(Vector3.up, 1f * Time.deltaTime * moveSpeed);
            distanceMoved += 1f * Time.deltaTime * moveSpeed;
        }
        #endregion

        //For testing purposes: restarts the minigame
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartMinigame();
        }

        if (distanceMoved >= 360f && !endMinigame)  // Determines when the minigame is over
        {
            EndMinigame();
        }
    }

    private void StartMinigame() // Restarts the minigame
    {
        endMinigame = false;  // Variable resets
        distanceMoved = 0f;  
        collectedPoints = 0f;
        scorePercentage = 0f;
        playerLimb.GetComponentInChildren<LimbCuttingScript>().DestroyPoints();  // Makes sure no points are left over
        playerLimb.GetComponentInChildren<LimbCuttingScript>().CreatePoints();  // Creates new points
    }

    private void EndMinigame()  // Ends the minigame
    {
        playerLimb.GetComponentInChildren<LimbCuttingScript>().DestroyPoints();
        CalculateScore();
        Debug.Log("Minigame Over! Score: " + scorePercentage + "%");
        endMinigame = true;
    }

    private void CalculateScore()  // Calculates the player's score based on collected points
    {
        scorePercentage = collectedPoints / startingNumberOfPoints * 100f;
    }

    private void OnTriggerEnter(Collider other)  // Detects when the player collects a point then destroys it
    {
        if (other.gameObject.CompareTag("LimbGamePoint"))
        {
            collectedPoints += 1f;
            Destroy(other.gameObject);
            Debug.Log("Collected Points: " + collectedPoints);
        }
    }
}
