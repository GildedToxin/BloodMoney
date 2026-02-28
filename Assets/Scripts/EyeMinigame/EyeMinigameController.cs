using System.Collections;
using System.Diagnostics;
using TMPro;
using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class EyeMinigameController : MonoBehaviour
{
    public bool miniGameRunning = true;

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

    public AudioClip startSFX;
    public GameObject StartCanvas;
    public Camera cam;
    private void Awake()
    {
        remainingTime = totalTime;
    }

    private void Start()
    {
     

    }

    public void Update()
    {
        if (miniGameRunning)
        {
            //for the crane mechanics 0 for vertical and 1 for horizontal. 3 stops the movement
            if (horizontalMovement == 0)
            {
                float y = Mathf.PingPong(Time.time * movementSpeed, 1) * maxY - minY;
                scoop.transform.position = new Vector3(scoop.transform.position.x, scoop.transform.position.y, y);
            }
            else if (horizontalMovement == 1)
            {
                float x = Mathf.PingPong(Time.time * movementSpeed, 1) * maxX - minX;
                scoop.transform.position = new Vector3(x, scoop.transform.position.y, scoop.transform.position.z);
            }

            // this swaps from vertical movement to horizontal
            if (Input.GetKeyDown(KeyCode.E) && horizontalMovement == 0)
            {
                scoop.transform.position = new Vector3(scoop.transform.position.x, scoop.transform.position.y, scoop.transform.position.z);
                horizontalMovement = 1;
            }
            else if (Input.GetKeyDown(KeyCode.E) && horizontalMovement == 1)
            {
                // this checks if the player enters the mash minigame or resets to the start
                if (scoopInPosition)
                {
                    mashMinigame.SetActive(true);
                    horizontalMovement = 3;
                    mm.isMashing = true;
                    mm.LockPosition();
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

    [ContextMenu("Move Body")]
    public void MoveBody()
    {
        float newX = Random.Range(minBodyX, maxBodyX);
        float newY = Random.Range(minBodyY, maxBodyY);
        Body.transform.position = new Vector3(newX, Body.transform.position.y, newY);
    }

     public void StartMiniGameRound()
    {
        StartCoroutine(StartMinigameRoundWithDelay());

    }
    public IEnumerator MoveBodyTest()
    {
        for (int i = 0; i < 100; i++)
        {
            float newX = Random.Range(minBodyX, maxBodyX);
            float newY = Random.Range(minBodyY, maxBodyY);
            Body.transform.position = new Vector3(newX, Body.transform.position.y, newY);
            yield return new WaitForSeconds(.1f);
        }
        yield return null;
    }
    public void StartMiniGame()
    {
        miniGameRunning = true;
        originalPosition = new Vector3(scoop.transform.position.x, scoop.transform.position.y, scoop.transform.position.z);
        timerText.text = string.Format("{00}", Mathf.FloorToInt(remainingTime % 60));
    }
    private IEnumerator StartMinigameRoundWithDelay()
    {

        AudioPool.Instance.PlayClip2D(startSFX);
        MoveBody();


        StartCanvas.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        UnityEngine.Debug.Log("Starting in 3...");
        yield return new WaitForSeconds(1f);
        StartCanvas.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        StartCanvas.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
        UnityEngine.Debug.Log("Starting in 2...");
        yield return new WaitForSeconds(1f);
        UnityEngine.Debug.Log("Starting in 1...");
        StartCanvas.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        StartCanvas.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);

        StartMiniGame();
        StartCanvas.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
        UnityEngine.Debug.Log("Mini-game started!");
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
    public void StopMiniGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        try
        {
            GameManager.Instance.StopMiniGame("EyeBallMinigame", cam);
            if (totalPoints > 0)
                FindAnyObjectByType<GameManager>().Body.SpawnOrgan("EyeBallMinigame");
            GameManager.Instance.Body.IsEyesHarvested = true;
            GameManager.Instance.Body.RemoveHighlight();
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError("Error ending minigame: " + e.Message);

        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //print(Cursor.visible);
    }
}
