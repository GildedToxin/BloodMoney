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


    // Minigame state variables
    private bool endMinigame = false;
    public float[] limbScores = new float[4];
    private int currentRound = 0; // Should go up to 4 (4 rounds 4 limbs)
    private Vector3 playerCursor;

    void Start()
    {
        startingNumberOfPoints = playerLimb.GetComponentInChildren<LimbCuttingScript>().numberOfPoints;
        currentRound = 0;
        playerCursor = this.transform.position;
    }

    void Update()
    {
        #region Player Input
        if (Input.GetKey(KeyCode.LeftArrow) && !endMinigame)
        {
            this.transform.position += Vector3.left * 0.1f * Time.deltaTime * (moveSpeed * 0.25f);
        }
        if (Input.GetKey(KeyCode.RightArrow) && !endMinigame)
        {
            this.transform.position += Vector3.right * 0.1f * Time.deltaTime * (moveSpeed * 0.25f);
        }
        if (Input.GetKey(KeyCode.UpArrow) && !endMinigame)
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

        if (distanceMoved >= 360f)  // Determines when the minigame is over
        {
            EndMinigameRound();
            if (currentRound < 4)
            {
                StartMinigameRound();
            }
        }
        
        if (currentRound > 4)  // Ends the entire minigame after all limbs are done
        {
            EndMinigame();
        }
    }

    private void StartMinigame() // Restarts the minigame
    {
        limbScores = new float[4];  // variable resets
        endMinigame = false;
        currentRound = 1;
        distanceMoved = 0f;
        collectedPoints = 0f;
        scorePercentage = 0f;
        this.transform.position = playerCursor;  // Resets player position
        playerLimb.GetComponentInChildren<LimbCuttingScript>().DestroyPoints();  // Makes sure no points are left over
        playerLimb.GetComponentInChildren<LimbCuttingScript>().CreatePoints();  // Creates new points
    }
    
    private void StartMinigameRound()  // Starts a new minigame round
    {
        currentRound += 1;
        distanceMoved = 0f;
        collectedPoints = 0f;
        scorePercentage = 0f;
        this.transform.position = playerCursor;  // Resets player position
        playerLimb.GetComponentInChildren<LimbCuttingScript>().DestroyPoints();  // Makes sure no points are left over
        playerLimb.GetComponentInChildren<LimbCuttingScript>().CreatePoints();  // Creates new points
    }

    private void EndMinigameRound()  // Ends the current minigame round
    {
        playerLimb.GetComponentInChildren<LimbCuttingScript>().DestroyPoints();
        CalculateScore();
        Debug.Log("Round Over! Score: " + scorePercentage + "%");
    }

    private void EndMinigame()  // Ends the entire minigame after all limbs are done
    {
        endMinigame = true;
        EndMinigameRound();
        Debug.Log(limbScores);
        // Additional logic for swapping cameras and other level based actions can be added here
    }

    private void CalculateScore()  // Calculates the player's score based on collected points
    {
        scorePercentage = collectedPoints / startingNumberOfPoints * 100f;

        // add scorePercentage to limbScores array
        limbScores[currentRound - 1] = scorePercentage;
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
