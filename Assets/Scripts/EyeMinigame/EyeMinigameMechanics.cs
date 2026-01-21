using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EyeMinigameMechanics : MonoBehaviour
{
    // core minigame variables
    [Header("Minigame Variables")]
    public float pickSpeed = 1.0f;
    public float totalTile = 5.0f;

    //Game Objects
    [Header("Minigame Objects")]
    public GameObject eyeball;
    public GameObject pick;

    //minigame inputs
    public 

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            pick.transform.position = new Vector3(0, 0, 0);
        }
    }
}
