using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MinigameFunctions : MonoBehaviour
{
    // core minigame variables
    [Tooltip("This variable needs to be 1 value under the minimum number of swings wanted. Not 0")]
    public int minHammerHits = 1;
    [Tooltip("This variable needs to be 1 value over the maximum number of swings wanted")]
    public int maxHammerHits = 5;
    private int numberOfHits = 0;
    private int CrackLevel = 0;
    private int neededHits;
    private bool minigameEnd = false;
    private bool timerStop = false;

    public List<GameObject> CrackStages;
    public GameObject winScreen;

    //timer variables
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;
    public float totalTime = 0;


    //win/lose countdown code
    [Tooltip("The ammount of time a player needs to wait before they win on the final stage")]
    public float winLoseTimerDuration = 3f;
    private float timerCountdown = 0f;
    [SerializeField] TextMeshProUGUI pointScoreText;

    // point system script
    public int qualityScore = 100;
    [Tooltip("Points removed per second after hitting the threshold")]
    public int timerPunishment = 3;
    [Tooltip("% of the timer left before deducting points")]
    public float timerThreshhold = 0.75f;
    

    private void Awake()
    {
        neededHits = Random.Range(minHammerHits, maxHammerHits);
        qualityScore = 100;
        totalTime = remainingTime;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            numberOfHits++;
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
            }

        }
        // lose conditions
        else if (CrackLevel == 4 && !minigameEnd)
        {
            timerStop = true;
            timerCountdown += Time.deltaTime;
            if (timerCountdown >= winLoseTimerDuration)
            {
                minigameEnd = true;
                pointDeduction();
                winScreen.SetActive(true);
            }
        }
        else if (remainingTime <= 0 && CrackLevel != 3 && !minigameEnd)
        {
            timerStop = true;
            minigameEnd = true;
            pointDeduction();
            winScreen.SetActive(true);
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
    }

    private void pointDeduction()
    {
        Debug.Log(remainingTime);
        Debug.Log(totalTime * timerThreshhold);
        if (remainingTime < totalTime  * timerThreshhold)
        {
            qualityScore = qualityScore - ((Mathf.RoundToInt(totalTime) - Mathf.RoundToInt(remainingTime)) * timerPunishment);
            pointScoreText.text = qualityScore.ToString();
        }
        else if (CrackLevel == 4)
        {
            qualityScore = 0;
            pointScoreText.text = ("Your Score is: " + qualityScore.ToString() + "% Quality!");
        }
        else
        {
            qualityScore = 100;
            pointScoreText.text = ("Your Score is: " + qualityScore.ToString() + "% Quality!");
        }
    }
}
