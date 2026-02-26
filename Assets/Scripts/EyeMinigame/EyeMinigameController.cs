using System.Diagnostics;
using TMPro;
using Unity.Hierarchy;
using UnityEngine;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class EyeMinigameController : MonoBehaviour
{
    private bool miniGameRunning = true;

    [Header("Body Values")]
    public GameObject Body;
    public float maxBodyY;
    public float minBodyY;
    public float maxBodyX;
    public float minBodyX;

    [Header("Scroll Values")]
    public int maxY;
    public int minY;
    public int maxX;
    public int minX;
    public float movementSpeed = 1f;
    private int horizontalMovement = 0;
    private Vector3 originalPosition;

    [Header("Scoop Objects")]
    public GameObject scoop;
    public bool scoopInPosition = false;
    public GameObject mashMinigame;

    [Header("OtherScript")]
    public mashingMinigame mm;

    [Header("timmer")]
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;
    public float totalTime;
    public bool timerStop = false;

    [Header("score System")]
    public int totalPoints = 100;
    public int timerPunishment = 2;
    [SerializeField] TextMeshProUGUI pointScoreText;
    public GameObject winScreen;

    private void Awake()
    {
        remainingTime = totalTime;
    }

    private void Start()
    {
        originalPosition = new Vector3 (scoop.transform.position.x, scoop.transform.position.y, scoop.transform.position.z);
        timerText.text = string.Format("{00}", Mathf.FloorToInt(remainingTime % 60));
        float newX = Random.Range(minBodyX, maxBodyX);
        float newY = Random.Range(minBodyY, maxBodyY);
        Body.transform.position = new Vector3(newX, newY, Body.transform.position.z);
    }

    public void Update()
    {
        if (miniGameRunning)
        {
            //for the crane mechanics 0 for vertical and 1 for horizontal. 3 stops the movement
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

            // this swaps from vertical movement to horizontal
            if (Input.GetKeyDown(KeyCode.Space) && horizontalMovement == 0)
            {
                scoop.transform.position = new Vector3(scoop.transform.position.x, scoop.transform.position.y, scoop.transform.position.z);
                horizontalMovement = 1;
            }
            else if (Input.GetKeyDown(KeyCode.Space) && horizontalMovement == 1)
            {
                // this checks if the player enters the mash minigame or resets to the start
                if (scoopInPosition)
                {
                    mashMinigame.SetActive(true);
                    horizontalMovement = 3;
                    mm.isMashing = true;
                }
                else
                {
                    scoop.transform.position = originalPosition;
                    horizontalMovement = 0;
                }
            }

            //this ticks down the timer
            if (!timerStop)
            {
                remainingTime -= Time.deltaTime;
                int seconds = Mathf.FloorToInt(remainingTime % 60);
                timerText.text = string.Format("{00}", seconds);
                if (remainingTime < 0)
                {
                    timerText.text = string.Format("{00}", 0);
                    timerStop = true;
                    miniGameRunning = false;
                    winGame();
                }
            }
        }
    }

    public void winGame()
    {
        winScreen.SetActive(true);
        // if time == 0 then they get no points, otherwise run the same calculations made for the skull minigame
        if (remainingTime < 0 && !miniGameRunning)
        {
            totalPoints = 0;
            pointScoreText.text = (totalPoints.ToString() + "%");
        }
        else
        {
            totalPoints = totalPoints - ((Mathf.RoundToInt(totalTime) - Mathf.RoundToInt(remainingTime)) * timerPunishment);
            pointScoreText.text = totalPoints.ToString() + "%";
        }
    }
}
