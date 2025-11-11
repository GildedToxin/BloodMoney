using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameFunctions : MonoBehaviour
{
    [Tooltip("This variable needs to be 1 value under the minimum number of swings wanted")]
    public int minHammerHits = 1;
    [Tooltip("This variable needs to be 1 value over the maximum number of swings wanted")]
    public int maxHammerHits = 5;
    private int numberOfHits = 0;
    private int CrackLevel = 0;
    private int neededHits;
    private bool minigameEnd = false;

    public List<GameObject> CrackStages;
    public GameObject winScreen;
    public GameObject loseScreen;
   

    //win countdown code
    [Tooltip("The ammount of time a player needs to wait before they win on the final stage")]
    public float winTimerDuration = 3f;
    private float winTimer = 0f;

    private void Awake()
    {
        neededHits = Random.Range(minHammerHits, maxHammerHits);
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

        if (CrackLevel == 3 && winTimer != winTimerDuration && !minigameEnd)
        {
            winTimer += Time.deltaTime;
            if (winTimer >= winTimerDuration)
            {
                winScreen.SetActive(true);
                minigameEnd= true;
            }

        }
        else if (CrackLevel == 4 && !minigameEnd)
        {
            loseScreen.SetActive(true);
            minigameEnd = true;
        }
    }

    private void NextCrackStage()
    {
        if (CrackLevel != 4)
        {
            CrackStages[CrackLevel].SetActive(false);
            CrackLevel++;
            CrackStages[CrackLevel].SetActive(true);
        }
    }
}
