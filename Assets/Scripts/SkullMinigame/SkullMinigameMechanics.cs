using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MinigameFunctions : MonoBehaviour
{
    [Tooltip("This variable needs to be 1 value under the minimum number of swings wanted")]
    public int minHammerHits = 1;
    [Tooltip("This variable needs to be 1 value over the maximum number of swings wanted")]
    public int maxHammerHits = 5;
    public int numberOfHits = 0;
    public int CrackLevel = 0;
    public int neededHits;

    public List<GameObject> CrackStages;
    public GameObject Hammer;


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
