using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Mono.Cecil;

public class BloodMinigameScript : MonoBehaviour
{
    public GameObject bloodDropStartPoint;
    public GameObject bloodDropEndPoint;
    public GameObject bloodDropEndPoint2;
    public GameObject playerHitZone;
    public GameObject bloodDropPrefab;
    public Slider scoreVisual;  // Displays score with a slider (should be replaced with proper visuals later)

 
    private float bloodDropTimer = 0.0f;   // Timer for spawning blood drops
    private float bloodDropTimeLimit;   // Adjust the LimitMin and Max to change spawn rate
    private float bloodDropTimeLimitMin = 0.5f; // Minimum time limit for spawning blood drops
    private float bloodDropTimeLimitMax = 2.0f; // Maximum time limit for spawning blood drops

    private bool gameRunning = false;
    private float bloodDropSpeed;
    private float bloodDropStartSpeed = 200.0f; // Starting speed for blood drops
    private float bloodDropMaxSpeed = 1000.0f; // Maximum speed for blood drops
    private float bloodSpeedIncreaseRate = 50.0f; // The rate at which blood drop speed increases over time
    public float minigameTime;  // Total time before minigame ends
    public float minigameStartTime = 40f;  // Change this to set the minigameTime start value for the beginning of the minigame

    // Blood drop spawn limits
    private float collectedBloodDrops = 0; // How many blood drops the player has collected
    private float totalBloodDrops = 10;  // Total blood drops the player needs to collect to get a max score
    public float score;
    public bool startGameBool = false; // make true in inspector to test minigame

    private List<GameObject> activeBloodDrops = new List<GameObject>();

    void Update()
    {

        if (gameRunning && minigameTime > 0)  // Main game loop
        {
            bloodDropTimer += Time.deltaTime;
            if (bloodDropTimer >= bloodDropTimeLimit)
                SpawnBloodDrop();
            MoveBloodDrops();
            SpeedUpBloodDrops();

            minigameTime -= Time.deltaTime;
            score = collectedBloodDrops / totalBloodDrops * 100;
            scoreVisual.value = score;
            if (gameRunning && Input.GetKeyDown(KeyCode.E))
            {
                CollectBloodDrop();
            }

            foreach(GameObject bloodDrop in activeBloodDrops)
            {
                if (Vector3.Distance(bloodDrop.GetComponent<RectTransform>().position,
                    playerHitZone.GetComponent<RectTransform>().position) < 80f)
                {
                    bloodDrop.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                }
                else { bloodDrop.transform.localScale = new Vector3(1f, 1f, 1f); }
            }
        }
        else if (gameRunning && minigameTime <= 0)
        {
            gameRunning = false;
        }

        // Stuff that should only happen once at the start of the minigame
        if (startGameBool)
        {
            StartGame();
            score = 0;
        }

        // Stuff that should only happen once at the end of the minigame
        if (gameRunning && minigameTime <= 0 || collectedBloodDrops >= totalBloodDrops)
        {
            if (collectedBloodDrops >= totalBloodDrops)
            {
                score = 100;
                scoreVisual.value = score;
            }
            EndGame();
        }
    }

    private void SpawnBloodDrop()
    {
        GameObject newBloodDrop = Instantiate(bloodDropPrefab, new Vector3(bloodDropStartPoint.transform.position.x, bloodDropStartPoint.transform.position.y, 0), Quaternion.identity);
        activeBloodDrops.Add(newBloodDrop);
        newBloodDrop.GetComponent<RectTransform>().SetParent(this.transform);
        bloodDropTimer = 0.0f; // Reset timer
        bloodDropTimeLimit = Random.Range(bloodDropTimeLimitMin, bloodDropTimeLimitMax); // Randomize next spawn time
        if (bloodDropSpeed > 600.0f)
            bloodDropTimeLimit /= 2.0f; // Spawn blood drops faster at higher speeds
    }

    private void MoveBloodDrops()
    {
        foreach (GameObject bloodDrop in activeBloodDrops)
        {
            bloodDrop.GetComponent<RectTransform>().position += new Vector3(-bloodDropSpeed * Time.deltaTime, 0, 0);
        }

        for (int i = activeBloodDrops.Count - 1; i >= 0; i--)
        {
            GameObject bloodDrop = activeBloodDrops[i];
            if (bloodDrop.GetComponent<RectTransform>().position.x <= bloodDropEndPoint.transform.position.x)
            {
                bloodDrop.transform.GetChild(0).localScale *= 0.99f;
                var image = bloodDrop.transform.GetChild(0).GetComponent<Image>();
                image.color = Color.Lerp(image.color, new Color(0, 0, 0, 0), 0.02f);
            }

            if (bloodDrop.GetComponent<RectTransform>().position.x <= bloodDropEndPoint2.transform.position.x)
            {
                bloodDrop.transform.GetChild(0).localScale *= 0.99f;
                var image = bloodDrop.transform.GetChild(0).GetComponent<Image>();
                image.color = Color.Lerp(image.color, new Color(0, 0, 0, 0), 0.02f);


                Destroy(bloodDrop);
                activeBloodDrops.RemoveAt(i);
                if (collectedBloodDrops > 0)
                    collectedBloodDrops--;
            }
        }
    }

    private void SpeedUpBloodDrops()
    {
        if (bloodDropSpeed < bloodDropMaxSpeed)
            bloodDropSpeed += Time.deltaTime * bloodSpeedIncreaseRate;
    }

    private void StartGame()
    {
        gameRunning = true;

        // Clear any existing blood drops
        for (int i = activeBloodDrops.Count - 1; i >= 0; i--)
        {
            GameObject bloodDrop = activeBloodDrops[i];
            Destroy(bloodDrop);
            activeBloodDrops.RemoveAt(i);
        }
        activeBloodDrops.Clear();

        // Reset Values
        bloodDropSpeed = bloodDropStartSpeed;
        collectedBloodDrops = 0;
        minigameTime = minigameStartTime;
        startGameBool = false;
        
    }

    private void EndGame()
    {
        gameRunning = false;
        startGameBool = false;
    }

    private void CollectBloodDrop()
    {
        GameObject closestBloodDrop = null;
        int closestBloodDropIndex = -1;
        for (int i = activeBloodDrops.Count - 1; i >= 0; i--)
        {
            GameObject bloodDrop = activeBloodDrops[i];
            float minDistance = 80f;

            // Checks for the closest blood drop in range
            if (Vector3.Distance(bloodDrop.GetComponent<RectTransform>().position, 
            playerHitZone.GetComponent<RectTransform>().position) < minDistance)
            {
                closestBloodDrop = bloodDrop;
                closestBloodDropIndex = i;
            }
        }
        if (closestBloodDropIndex == -1) // When there is no blood drop close enough to collect
        {
            if (collectedBloodDrops > 0)  // Reduces the score if the player miss times their hit
                collectedBloodDrops--;
            return;
        }
        Destroy(closestBloodDrop);
        activeBloodDrops.RemoveAt(closestBloodDropIndex);
        collectedBloodDrops++;
    }
}
