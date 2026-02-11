using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MinigameFunctions : MonoBehaviour
{
    // core minigame variables
    [Header("Minigame Variables")]
    [Tooltip("This variable needs to be 1 value under the minimum number of swings wanted. Not 0")]
    public int minHammerHits = 1;
    [Tooltip("This variable needs to be 1 value over the maximum number of swings wanted")]
    public int maxHammerHits = 5;
    private int numberOfHits = 0;
    private int CrackLevel = 0;
    private int neededHits;
    private bool minigameEnd = false;
    private bool timerStop = false;

    public bool isMinigameActive = false;

    [Header("Assets")]
    public List<GameObject> CrackStages;
    public GameObject winScreen;
    public Camera cam;
    public GameObject StartCanvas;
    public GameObject hammer;
    public GameObject brain;
    public GameObject blood;


    [Header("Timer Values")]
    //timer variables
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;


    //win/lose countdown code
    [Tooltip("The ammount of time a player needs to wait before they win on the final stage")]
    public float winLoseTimerDuration = 3f;
    private float timerCountdown = 0f;
    public float totalTime = 0;

    [Header("Score Values")]
    // point system script
    public int qualityScore = 100;
    [Tooltip("Points removed per second after hitting the threshold")]
    public int timerPunishment = 3;
    [Tooltip("% of the timer left before deducting points")]
    public float timerThreshhold = 0.75f;
    [SerializeField] TextMeshProUGUI pointScoreText;

    [Header("Hammer Animations")]
    //Hammer animations
    public Animator HammerAni;
    private bool isAnimated = false;


    public Material bloodyMat;

    private void Awake()
    {
        neededHits = Random.Range(minHammerHits, maxHammerHits);
        qualityScore = 100;
        totalTime = remainingTime;
    }
    private void Start()
    {
        StartCoroutine(StartMinigameRoundWithDelay());
        timerText.text = string.Format("{00}", Mathf.FloorToInt(remainingTime % 60));
    }
    private void Update()
    {
        if(!isMinigameActive)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            HammerAni.Play("Hammer_Swing", 0, 0.25f);
            numberOfHits++;
            HammerAni.ResetTrigger("Hammer_Swing");
        }

        if (numberOfHits == neededHits)
        {
            numberOfHits = 0;
            neededHits = Random.Range(minHammerHits, maxHammerHits);
            NextCrackStage();

        }

        //win conditions
        if (CrackLevel == 3 && timerCountdown != winLoseTimerDuration && !minigameEnd)
        {
            timerCountdown += Time.deltaTime;
            if (timerCountdown >= winLoseTimerDuration)
            {
                timerStop = true;
                pointDeduction();
                winScreen.SetActive(true);
                minigameEnd = true;

                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                isMinigameActive = false;
            }

        }
        // lose conditions
        else if (CrackLevel == 4 && !minigameEnd)
        {
            brain.SetActive(false);
            blood.SetActive(true);
            timerStop = true;
            timerCountdown += Time.deltaTime;
            if (timerCountdown >= winLoseTimerDuration)
            {
                minigameEnd = true;
                pointDeduction();
                winScreen.SetActive(true);

                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                isMinigameActive = false;
            }
        }
        else if (remainingTime <= 0 && CrackLevel != 3 && !minigameEnd)
        {
            timerStop = true;
            minigameEnd = true;
            pointDeduction();
            winScreen.SetActive(true);

            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            isMinigameActive = false;
        }

        //timer countdown script
        if (!timerStop)
        {
            remainingTime -= Time.deltaTime;
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            timerText.text = string.Format("{00}", seconds);
        }
    }


    //changes model to the next crack stage
    private void NextCrackStage()
    {
        if (CrackLevel != 4)
        {
            CrackStages[CrackLevel].SetActive(false);
            CrackLevel++;
            CrackStages[CrackLevel].SetActive(true);
        }
        
        if (CrackLevel == 3)
        {
            neededHits = 1;
        }

        if (CrackLevel == 2)
        {
            hammer.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = bloodyMat;
            hammer.transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material = bloodyMat;
        }
    }

    private void pointDeduction()
    {
        Debug.Log(remainingTime);
        Debug.Log(totalTime * timerThreshhold);
        if (CrackLevel >= 4)
        {
            qualityScore = 0;
            pointScoreText.text = (qualityScore.ToString() + "%");
        }
        else if(remainingTime < totalTime * timerThreshhold)
        {
            qualityScore = qualityScore - ((Mathf.RoundToInt(totalTime) - Mathf.RoundToInt(remainingTime)) * timerPunishment);
            pointScoreText.text = qualityScore.ToString() + "%";
        }
        else
        {
            qualityScore = 100;
            pointScoreText.text = (qualityScore.ToString() + "%");
        }
    }

    public void StopMiniGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        try
        {
            GameManager.Instance.StopMiniGame("SkullMinigame", cam);
            if(qualityScore > 0)
                FindAnyObjectByType<GameManager>().Body.SpawnOrgan("SkullMinigame");
            else
            {
                Destroy(FindAnyObjectByType<GameManager>().Body.BodyBrain);
                Destroy(FindAnyObjectByType<GameManager>().Body.headTop);
            }
                GameManager.Instance.Body.IsBrainHarvested = true;
            GameManager.Instance.Body.RemoveHighlight();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error ending minigame: " + e.Message);

        }
    }

    private IEnumerator StartMinigameRoundWithDelay()
    {

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

        isMinigameActive = true;
        StartCanvas.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
        Debug.Log("Mini-game started!");
    }
}
