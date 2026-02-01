using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BloodMinigameScript : MonoBehaviour
{
    public GameObject bloodDropStartPoint;
    public GameObject bloodDropEndPoint;
    public GameObject playerHitZone;
    public GameObject bloodDropPrefab;
    public GameObject scoreVisual;  // Displays score with a slider (should be replaced with proper visuals later)

 
    private float bloodDropTimer = 0.0f;   // Timer for spawning blood drops
    private float bloodDropTimeLimit = 3.0f;

    private bool gameRunning = false;
    private float bloodDropSpeed = 200.0f;
    public float minigameTime = 40f;  // Total time before minigame ends

    // Blood drop spawn limits
    private int maxBloodDrops = 15; 
    private int minBloodDrops = 10; 
    private int currentBloodDrops = 0; // How many blood drops are left to spawn
    private int collectedBloodDrops = 0; // How many blood drops the player has collected
    private int totalBloodDrops = 0;  // Total blood drops to be spawned in the minigame
    private int score = 0;
    public bool startGameBool = false; // make true in inspector to test minigame

    private List<GameObject> activeBloodDrops = new List<GameObject>();
    void Start()
    {
        
    }

    void Update()
    {
        if (gameRunning && minigameTime > 0)
        {
            bloodDropTimer += Time.deltaTime;
            if (bloodDropTimer >= bloodDropTimeLimit)
                SpawnBloodDrop();
            MoveBloodDrops();
            SpeedUpBloodDrops();

            minigameTime -= Time.deltaTime;
            score = collectedBloodDrops / totalBloodDrops * 100;
            scoreVisual.GetComponent<UnityEngine.UI.Slider>().value = score;
            if (gameRunning && Input.GetKeyDown(KeyCode.E))
            {
                CollectBloodDrop();
            }
        }
        else if (gameRunning && minigameTime <= 0)
        {
            gameRunning = false;
        }

        if (startGameBool)
        {
            StartGame();
        }

        if (gameRunning && minigameTime <= 0)
        {
            EndGame();
        }
    }

    private void SpawnBloodDrop()
    {
        GameObject newBloodDrop = Instantiate(bloodDropPrefab, new Vector3(bloodDropStartPoint.transform.position.x, bloodDropStartPoint.transform.position.y, 0), Quaternion.identity);
        activeBloodDrops.Add(newBloodDrop);
        newBloodDrop.GetComponent<RectTransform>().SetParent(this.transform);
        bloodDropTimer = 0.0f;
        currentBloodDrops--;
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
                Destroy(bloodDrop);
                activeBloodDrops.RemoveAt(i);
            }
        }
    }

    private void SpeedUpBloodDrops()
    {
        if (bloodDropSpeed < 1000.0f)
            bloodDropSpeed += Time.deltaTime * 50.0f;
    }

    private void StartGame()
    {
        gameRunning = true;
        currentBloodDrops = Random.Range(minBloodDrops, maxBloodDrops + 1);
        totalBloodDrops = currentBloodDrops;

        // Clear any existing blood drops
        for (int i = activeBloodDrops.Count - 1; i >= 0; i--)
        {
            GameObject bloodDrop = activeBloodDrops[i];
            Destroy(bloodDrop);
            activeBloodDrops.RemoveAt(i);
        }
        activeBloodDrops.Clear();

        // Reset Values
        bloodDropSpeed = 200.0f;
        collectedBloodDrops = 0;
        minigameTime = 30f;
        startGameBool = false;
        score = 0;
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
            float minDistance = 50f;

            if (Vector3.Distance(bloodDrop.GetComponent<RectTransform>().position, playerHitZone.GetComponent<RectTransform>().position) < minDistance)
            {
                closestBloodDrop = bloodDrop;
                closestBloodDropIndex = i;
            }
        }
        if (closestBloodDropIndex == -1)
            return;
        Destroy(closestBloodDrop);
        activeBloodDrops.RemoveAt(closestBloodDropIndex);
        collectedBloodDrops++;
    }
}
