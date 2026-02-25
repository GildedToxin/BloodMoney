using UnityEngine;
using UnityEngine.UI;

public class BoneCuttingMiniGameScript : MonoBehaviour
{
    [SerializeField] private GameObject topArea; // Top bounds of minigame
    [SerializeField] private GameObject bottomArea; // bottom bounds of minigame
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject targetArea; // object the player must touch to gain score
    [SerializeField] private Slider scoreVisual; // Whitebox slider for progression visual
    [SerializeField] private BoneMiniGameScript mainScript; // Reference to the main script for the mini game
    [SerializeField] public bool startGame = false;

    private float playerTargetRange = 25f; // how far the player must be from the target to gain points
    private bool playerInput = false;
    [SerializeField] private float score = 0; // current score
    [SerializeField] private float scoringRate = 20f; // the speed that the score goes up when the player is in range
    private float maxScore = 100f;

    void Start()
    {
        
    }

    void Update()
    {
        if (startGame)
        {
            // checks the players y pos against the bottom bounds and applies "gravity" to the player
            Gravity();

            // clamps the player's position
            if (player.GetComponent<RectTransform>().position.y < bottomArea.GetComponent<RectTransform>().position.y || 
            player.GetComponent<RectTransform>().position.y > topArea.GetComponent<RectTransform>().position.y)
                Mathf.Clamp(player.GetComponent<RectTransform>().position.y, bottomArea.GetComponent<RectTransform>().position.y, topArea.GetComponent<RectTransform>().position.y);
            
            // player input
            PlayerInput();
            GatheringPoints();

            if (score < maxScore) scoreVisual.value = score; // Using slider for whiteboxing the score progress
            if (score >= maxScore) 
            {
                mainScript.currentBone++;
                startGame = false;
            }
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
}
