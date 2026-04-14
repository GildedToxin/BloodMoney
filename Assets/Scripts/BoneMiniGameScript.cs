using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms.Impl;
using TMPro;

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
    public GameObject EndCanvas;
    public GameObject currentBoneGO;
    public Camera cam;


    public AudioClip winSFX;
    public AudioClip loseSFX;

    public AudioClip readySFX;
    public AudioClip goSFX;
    public AudioClip boneColorSFX;
    public AudioClip wrongBoneSFX;
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

        FindAnyObjectByType<BoneMouseScript>().enabled = true;
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
                {
                    AudioPool.Instance.PlayClip2D(boneColorSFX);
                    StartCuttingGame();
                    cuttingScript.reel.Play();
                }
                else
                {
                    numberOfIncorrect++;
                    AudioPool.Instance.PlayClip2D(wrongBoneSFX);
                }
            }
                
        }
    }

    private void StartCuttingGame()
    {
        failedCutting = false;
        cuttingScript.startGame = true;
        FindAnyObjectByType<BoneMouseScript>().enabled = false;
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

        EndCanvas.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = Mathf.Round(scoreAverage) + "%";
        EndCanvas.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        if(scoreAverage > 0)
        {
            AudioPool.Instance.PlayClip2D(winSFX);
        }
        else
        {
            AudioPool.Instance.PlayClip2D(loseSFX);
        }
    }

    [ContextMenu("Stop MiniGame")]
    public void StopMiniGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        try
        {

            GameManager.Instance.StopMiniGame("BoneMiniGame", cam);
            if (scoreAverage > 0)
                FindAnyObjectByType<GameManager>().Body.SpawnOrgan("BoneMiniGame", (int)scoreAverage);
            GameManager.Instance.Body.IsBonesHarvested = true;
            Destroy(GameManager.Instance.Body.Chest);
            GameManager.Instance.Body.RemoveHighlight();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error ending minigame: " + e.Message);

        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        print(Cursor.visible);
    }
    public void ShowStartGame()
    {
        StartCoroutine(StartMinigameRoundWithDelay());
    }
    private IEnumerator StartMinigameRoundWithDelay()
    {
        Cursor.visible = false;
        Cursor.lockState= CursorLockMode.Locked;  
        //AudioPool.Instance.PlayClip2D(startSFX);
        //transform.GetChild(0).gameObject.SetActive(true);
        //transform.GetChild(2).gameObject.SetActive(true);
        StartCanvas.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        Debug.Log("Starting in 3...");
        AudioPool.Instance.PlayClip2D(readySFX);
        yield return new WaitForSeconds(1f);
        StartCanvas.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        StartCanvas.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
        Debug.Log("Starting in 2...");
        AudioPool.Instance.PlayClip2D(readySFX);
        yield return new WaitForSeconds(1f);
        StartCanvas.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);

        RandomizeBones();

        foreach(int boneNum in boneOrder)
        {
            boneObjects[boneNum - 1].transform.GetChild(1).gameObject.SetActive(true);
            AudioPool.Instance.PlayClip2D(boneColorSFX);
            yield return new WaitForSeconds(.5f);
            boneObjects[boneNum - 1].transform.GetChild(1).gameObject.SetActive(false);
        }




        Debug.Log("Starting in 1...");

        StartCanvas.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
        AudioPool.Instance.PlayClip2D(goSFX);
        yield return new WaitForSeconds(1f);

        
        StartCanvas.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
        Debug.Log("Mini-game started!");
        StartGame();
    }
    
}
