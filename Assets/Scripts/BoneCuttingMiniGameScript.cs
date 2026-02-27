using System;
using UnityEngine;
using UnityEngine.UI;

public class BoneCuttingMiniGameScript : MonoBehaviour
{
    [SerializeField] private GameObject UserInterface; // enabled and disabled to hide cutting UI until the bone is selected
    [SerializeField] private GameObject topArea; // Top bounds of minigame
    [SerializeField] private GameObject bottomArea; // bottom bounds of minigame
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject targetArea; // object the player must touch to gain score
    [SerializeField] private Slider scoreVisual; // Whitebox slider for progression visual
    [SerializeField] private BoneMiniGameScript mainScript; // Reference to the main script for the mini game
    [SerializeField] public bool startGame = false;
    private bool endGame = false;

    private Vector3 initialPlayerPos;
    private Vector3 initialTargetPos;
    private Vector3 targetNewPos;
    private float playerTargetRange = 25f; // how far the player must be from the target to gain points
    private bool playerInput = false;
    [SerializeField] private float score = 0; // current score
    [SerializeField] private float scoringRate = 20f; // the speed that the score goes up when the player is in range
    private float maxScore = 100f;
    private float startTime = 60f;  // Set this to set the time limit of the cutting minigame
    public float timer = 0f;

    void Start()
    {
        initialPlayerPos = player.GetComponent<RectTransform>().position;
        initialTargetPos = targetArea.GetComponent<RectTransform>().position;
        targetNewPos = initialTargetPos;
        timer = startTime;
        UserInterface.SetActive(false);
    }

    void Update()
    {
        if (startGame)
        {
            UserInterface.SetActive(true);
            // checks the players y pos against the bottom bounds and applies "gravity" to the player
            Gravity();

            // clamps the player's position
            if (player.GetComponent<RectTransform>().position.y < bottomArea.GetComponent<RectTransform>().position.y || 
            player.GetComponent<RectTransform>().position.y > topArea.GetComponent<RectTransform>().position.y)
                Mathf.Clamp(player.GetComponent<RectTransform>().position.y, bottomArea.GetComponent<RectTransform>().position.y, topArea.GetComponent<RectTransform>().position.y);
            
            // player input
            PlayerInput();
            GatheringPoints();

            // Target area movement
            MoveTarget();

            if (score < maxScore) scoreVisual.value = score; // Using slider for whiteboxing the score progress

            // End game conditions
            if (score >= maxScore) // winning
            {
                EndGame();
            }
            }
            if (startGame) // Timer
                timer -= Time.deltaTime;
            if (timer <= 0) // losing (due to running out of time)
            {
                mainScript.failedCutting = true;
                EndGame();
            }
    }

    private void PlayerInput()
    {
        if (Input.GetKey(KeyCode.E))
            {
                Vector3 playerFallVector = new Vector3(0, 200, 0);
                player.GetComponent<RectTransform>().position += playerFallVector * Time.deltaTime;
                playerInput = true;
            }
            else 
                playerInput = false;
    }

    private void Gravity()
    {
        if (player.GetComponent<RectTransform>().position.y > bottomArea.GetComponent<RectTransform>().position.y && !playerInput)
            {
                Vector3 playerSpeedVector = new Vector3(0, 200, 0);
                player.GetComponent<RectTransform>().position -= playerSpeedVector * Time.deltaTime;
            }
    }

    private void GatheringPoints()
    {
        if (Vector3.Distance(player.GetComponent<RectTransform>().position, targetArea.GetComponent<RectTransform>().position) < playerTargetRange)
        {
            score += scoringRate * Time.deltaTime;
        }
    }

    private void ResetValues()
    {
        score = 0;
        player.GetComponent<RectTransform>().position = initialPlayerPos;
        targetArea.GetComponent<RectTransform>().position = initialTargetPos;
        timer = startTime;
    }

    private void MoveTarget()
    {
        // Selects new position once it reaches the new position
        if (Vector3.Distance(targetArea.GetComponent<RectTransform>().position, targetNewPos) <= 10f)
        {
            Vector3 newerPosition = new Vector3(0, UnityEngine.Random.Range(bottomArea.GetComponent<RectTransform>().position.y + 30, topArea.GetComponent<RectTransform>().position.y - 30));
            targetNewPos = newerPosition;
        }
        Vector3.Lerp(targetArea.GetComponent<RectTransform>().position, targetNewPos, 5 * Time.deltaTime);
    }

    private void EndGame()
    {
        if (mainScript.failedCutting)
            mainScript.scores[mainScript.currentBone - 1] = 100 - (mainScript.numberOfIncorrect * mainScript.pointsDamage) - mainScript.failedCuttingDamage;
        else
            mainScript.scores[mainScript.currentBone - 1] = 100 - (mainScript.numberOfIncorrect * mainScript.pointsDamage);
        Mathf.Clamp(mainScript.scores[mainScript.currentBone - 1], 0, 100);
        mainScript.numberOfIncorrect = 0;
        mainScript.currentBone++;
        startGame = false;
        ResetValues();
        UserInterface.SetActive(false);
    }
}
