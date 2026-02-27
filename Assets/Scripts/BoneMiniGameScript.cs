using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoneMiniGameScript : MonoBehaviour
{
    [SerializeField] private List<int> boneOrder = new List<int> { 1, 2, 3, 4, 5, 6 };
    [SerializeField] private List<GameObject> boneObjects = new List<GameObject>();
    [SerializeField] private int selectedBoneIndex = 0;
    private BoneCuttingMiniGameScript cuttingScript;
    [SerializeField] public int currentBone = 1; // Which bone in the order the player is at (starts at 1 ends at 6)
    public bool start = false;
    private bool gameStarted = false;
    private float boneDamage = 10f; // how much damage the bone takes when selecting the wrong bone

    public LayerMask hitLayers; // Use organs layer
    private float maxDistance = Mathf.Infinity;

    public List<float> scores = new List<float> { 0, 0, 0, 0, 0, 0 };
    [SerializeField] private float currentScore = 100f;
    [HideInInspector] public float pointsDamage = 10f;  // How many points the player loses when selecting the wrong bone
    [HideInInspector] public int numberOfIncorrect = 0;
    public float scoreAverage = 0f;  // Average of all the scores in the scores list
    public bool failedCutting = false;  // Becomes true if the player runs out of time during the cutting
    [HideInInspector] public float failedCuttingDamage = 30f; // multiplies each score that was failed with this number
    private bool enabledInput = false; // determines when to begin using input at the beginning of the game after highlighting objects

    void Start()
    {
        cuttingScript = this.gameObject.GetComponent<BoneCuttingMiniGameScript>();
        enabledInput = true;
    }

    void Update()
    {
        if (start == true)
        {
            StartGame();

        }

        // Raycast out from camera from mouse cursor to highlight bones
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance, hitLayers) && selectedBoneIndex != currentBone && enabledInput)
        {
            Debug.Log("Hit object: " + hit.collider.gameObject.name);
        }


        // Actions that should happen when left clicking (use to select bone)
        if (Input.GetMouseButtonDown(0) && selectedBoneIndex != currentBone && enabledInput)
        {
            SelectBone();
        }
        if (selectedBoneIndex == currentBone)
        {
            StartCuttingGame();
        }

        // Endgame logic and scoring
        if (currentBone >= 7)
            EndGame();
    }

    private void StartGame()
    {
        // Randomizes the order of ribs to cut
        for (int i = boneOrder.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int temp = boneOrder[i];
            boneOrder[i] = boneOrder[j];
            boneOrder[j] = temp;
        }
        start = false;
        selectedBoneIndex = 0;
        currentBone = 1;

        // Resets scores
        for (int i = 0; i < scores.Count; i++)
        {
            scores[i] = 0f;
        }
        scoreAverage = 0f;
    }

    private void SelectBone()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance, hitLayers))
        {
            if (boneObjects.Contains(hit.collider.gameObject))
            {
                selectedBoneIndex = boneOrder[boneObjects.IndexOf(hit.collider.gameObject)];
            }
            if (selectedBoneIndex != currentBone)
                numberOfIncorrect++;
        }
    }

    private void StartCuttingGame()
    {
        failedCutting = false;
        cuttingScript.startGame = true;
    }

    private void EndGame()
    {   
        currentBone = 1;
        // Handle scoring and value resets here
        foreach (float score in scores)
        {
            scoreAverage = scoreAverage + score;
            Debug.Log(scoreAverage);
        }
        scoreAverage = scoreAverage / 6;
    }
}
