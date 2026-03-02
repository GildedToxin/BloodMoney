using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoneMiniGameScript : MonoBehaviour
{
    [SerializeField] private Material highlightMat;
    private int highlightNum; // used to determine with object should be highlighted
    [SerializeField] private List<int> boneOrder = new List<int> { 1, 2, 3, 4, 5, 6 };
    [SerializeField] public List<GameObject> boneObjects = new List<GameObject>();
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

    public GameObject StartCanvas;
    public GameObject currentBoneGO;
    void Start()
    {
        cuttingScript = this.gameObject.GetComponent<BoneCuttingMiniGameScript>();
        //enabledInput = true;
    }

    void Update()
    {
        

        // Raycast out from camera from mouse cursor to highlight bones
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance, hitLayers) && selectedBoneIndex != currentBone && enabledInput)
        {
            Debug.Log("Hit object: " + hit.collider.gameObject.name);
        }


        // Actions that should happen when left clicking (use to select bone)
        if (Input.GetMouseButtonDown(0) /*&& selectedBoneIndex != currentBone */ && enabledInput)
        {
            SelectBone();
        }
      //  if (selectedBoneIndex == currentBone)
       // {
       //     StartCuttingGame();
       // }

        // Endgame logic and scoring
        if (currentBone >= 7)
            EndGame();
    }

    private void StartGame()
    {
        selectedBoneIndex = 0;
        currentBone = 1;
        enabledInput = true;

        // Resets scores
        for (int i = 0; i < scores.Count; i++)
        {
            scores[i] = 0f;
        }
        scoreAverage = 0f;
    }
    public void RandomizeBones()
    {
        // Randomizes the order of ribs to cut
        for (int i = boneOrder.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int temp = boneOrder[i];
            boneOrder[i] = boneOrder[j];
            boneOrder[j] = temp;
        }
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
                currentBoneGO = hit.collider.gameObject;

                if (boneObjects.IndexOf(hit.collider.gameObject) + 1 == boneOrder[currentBone - 1])
                    StartCuttingGame();
                else
                    numberOfIncorrect++;
            }
                
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
        }
        scoreAverage = scoreAverage / 6;
    }

 
    public void ShowStartGame()
    {
        StartCoroutine(StartMinigameRoundWithDelay());
    }
    private IEnumerator StartMinigameRoundWithDelay()
    {
        //AudioPool.Instance.PlayClip2D(startSFX);
        //transform.GetChild(0).gameObject.SetActive(true);
        //transform.GetChild(2).gameObject.SetActive(true);
        StartCanvas.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        Debug.Log("Starting in 3...");
        yield return new WaitForSeconds(1f);
        StartCanvas.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        StartCanvas.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
        Debug.Log("Starting in 2...");
        yield return new WaitForSeconds(1f);
        StartCanvas.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);

        RandomizeBones();

        foreach(int boneNum in boneOrder)
        {
            boneObjects[boneNum - 1].transform.GetChild(1).gameObject.SetActive(true);
            yield return new WaitForSeconds(.5f);
            boneObjects[boneNum - 1].transform.GetChild(1).gameObject.SetActive(false);
        }




        Debug.Log("Starting in 1...");

        StartCanvas.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);

        
        StartCanvas.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
        Debug.Log("Mini-game started!");
        StartGame();
    }
    
}
