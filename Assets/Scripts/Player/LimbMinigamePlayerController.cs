using System.Collections; // Required for coroutines
using UnityEngine;

public class LimbMinigamePlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject playerLimb;
    [SerializeField] private float moveSpeed = 30f;
    [SerializeField] private float spinSpeed = 30f;

    [SerializeField]
    private float collectedPoints = 0f;
    private float scorePercentage = 0f;
    [SerializeField]  private float distanceMoved = 0f;
    private float startingNumberOfPoints = 0f;


    // Minigame state variables
    private bool endMinigame = false;
    public float[] limbScores = new float[4];
    private int currentRound = 0; // Should go up to 4 (4 rounds 4 limbs)
    private Vector3 playerCursor;

    // Timer variables
    public float limbTimer = 0f;
    private float limbTimerLimit = 30f; // Time limit for each round
    
    
    public Camera cam;
    public LimbEndScreen limbEndScreen;
    public GameObject StartCanvas;
    public bool isMinigameActive = false;

    void Start()
    {
        startingNumberOfPoints = playerLimb.GetComponentInChildren<LimbCuttingScript>().numberOfPoints;
        currentRound = 0;
        playerCursor = this.transform.position;

        // Start the 3 second timer before starting the mini-game
        StartMinigame();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            distanceMoved = 0f;
            collectedPoints = 0f;
            scorePercentage = 0f;
            this.transform.position = playerCursor;  // Resets player position
            playerLimb.GetComponentInChildren<LimbCuttingScript>().DestroyPoints();  // Makes sure no points are left over
            playerLimb.GetComponentInChildren<LimbCuttingScript>().CreatePoints();  // Creates new points
        }

        if (isMinigameActive)
        {
            #region Player Input
            if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && !endMinigame)
            {
                float newX = transform.position.x - 0.1f * Time.deltaTime * (moveSpeed * 0.25f);
                newX = Mathf.Clamp(newX, -1.1f, .3f);

                transform.position = new Vector3(newX, transform.position.y, transform.position.z);
            }
            if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && !endMinigame)
            {
                float newX = transform.position.x + 0.1f * Time.deltaTime * (moveSpeed * 0.25f);
                newX = Mathf.Clamp(newX, -1.1f, .3f);

                transform.position = new Vector3(newX, transform.position.y, transform.position.z);

            }
                playerLimb.transform.Rotate(Vector3.up, 1f * Time.deltaTime * spinSpeed);
                distanceMoved += 1f * Time.deltaTime * spinSpeed;
            #endregion
        }
        if ((distanceMoved >= 360f || limbTimer > limbTimerLimit) && isMinigameActive)  // Determines when the minigame is over
        {
            EndMinigameRound();
            if (currentRound < 4)
            {
                StartCoroutine(StartMinigameRoundWithDelay());
            }
            else
            {
                limbEndScreen.gameObject.SetActive(true);
                limbEndScreen.UpdateText(limbScores);
                //StopMiniGame();
            }
            isMinigameActive = false;
        }

        if (currentRound > 4)  // Ends the entire minigame after all limbs are done
        {
            EndMinigame();
        }
        
        // Timer
        if (!endMinigame && currentRound > 0)
        {
            limbTimer += Time.deltaTime;
        }
    }

    private IEnumerator StartMinigameRoundWithDelay()
    {

        this.transform.position = playerCursor;  // Resets player position
        playerLimb.GetComponentInChildren<LimbCuttingScript>().DestroyPoints();  // Makes sure no points are left over
        playerLimb.GetComponentInChildren<LimbCuttingScript>().CreatePoints();  // Creates new points


        StartCanvas.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        Debug.Log("Starting in 3...");
        yield return new WaitForSeconds(1f);
        StartCanvas.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        StartCanvas.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
        Debug.Log("Starting in 2...");
        yield return new WaitForSeconds(1f);
        Debug.Log("Starting in 1...");
        StartCanvas.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        StartCanvas.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);

        StartMinigameRound();
        StartCanvas.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
        Debug.Log("Mini-game started!");
    }

    private void StartMinigame() // Restarts the minigame
    {
        limbScores = new float[4];  
        endMinigame = false;
        currentRound = 0;

        StartCoroutine(StartMinigameRoundWithDelay());
    }
    
    private void StartMinigameRound()  // Starts a new minigame round
    {
        isMinigameActive = true;
        currentRound += 1;
        limbTimer = 0f;
        distanceMoved = 0f;
        collectedPoints = 0f;
        scorePercentage = 0f;
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
            //Debug.Log("Collected Points: " + collectedPoints);
        }
    }

    public void StopMiniGame()
    {
        try
        {
            FindAnyObjectByType<GameManager>().StopMiniGame("LimbMiniGame", cam);
            FindAnyObjectByType<GameManager>().Body.IsLimbsHarvested = true;
            FindAnyObjectByType<GameManager>().Body.RemoveHighlight();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error ending minigame: " + e.Message);

        }
    }
}
