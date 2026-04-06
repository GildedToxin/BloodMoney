using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;


public class HandFeetMinigameManager : MonoBehaviour
{
    public List<GameObject> limbs = new List<GameObject>();
    public List<GameObject> mazes = new List<GameObject>();
    public List<GameObject> pointerHolder = new List<GameObject>();
    public List<GameObject> endpoints = new List<GameObject>();
    public int currentMaze = 0;

    private Vector3 position;
    public float offset = 3f;

    RaycastHit hit;
    Ray ray;

    public MouseFollower mouseFollower;
    public GameObject raycastHolder;

    public float score = 100f;
    public float pointDeduction = 0.2f;

    public bool minigameEnd = false;

    [Header("timmer")]
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;
    public float totalTime;
    public bool timerStop = false;

    [SerializeField] TextMeshProUGUI winText;
    public GameObject winScreen;


    public Camera cam;
    public Transform target;

    public GameObject StartCanvas;
    public AudioClip startSFX;

    public bool isInMiniGame = false;

    public void Start()
    {
  


    }

    public void Update()
    {
        if(minigameEnd && winScreen.activeSelf)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        if (!isInMiniGame)
        {

            return;
        }
        position = Input.mousePosition;
        position.z = offset;

        if (mouseFollower.follow && !minigameEnd)
        {
            Debug.DrawRay(raycastHolder.transform.position, Vector3.forward * 5, Color.green);
            if (Physics.Raycast(raycastHolder.transform.position, Vector3.forward * 5, out hit))
            {
                if (hit.collider.gameObject.name == endpoints[currentMaze].gameObject.name)
                {
                    EndMinigame();
                }
            }
            else
            {
                score -= pointDeduction;
                if (score < 0)
                {
                    score = 0;
                    EndMinigame();
                }
            }
        }

        if (!timerStop)
        {
            remainingTime -= Time.deltaTime;
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            timerText.text = string.Format("{00}", seconds);
            if (remainingTime < 0)
            {
                timerText.text = string.Format("{00}", 0);
                timerStop = true;
                minigameEnd = true;
                score = 0;
                EndMinigame();
            }
        }
    }
    private void EndMinigame()
    {
        timerStop = true;
        minigameEnd = true;
        score = Mathf.Round(score);
        winText.text = score.ToString() + "%";
        winScreen.SetActive(true);
        FindAnyObjectByType<BoneMouseScript>().enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void StopMiniGame()
    {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        try
        {
            GameManager.Instance.StopMiniGame("HandMinigame", cam);
            if (score > 0)
                FindAnyObjectByType<GameManager>().Body.SpawnOrgan("HandMinigame", (int)score);



            var body = FindAnyObjectByType<GameManager>().Body;

            body.handsHarvested++;
            if (body.handsHarvested >= 4)
            {
                body.IsFingersHarvested = true;
            }

            //This needs to be changed, 4 hands like limbs
          //  GameManager.Instance.Body.RemoveHighlight();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error ending minigame: " + e.Message);

        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        print(Cursor.visible);
    }
    public void StartGame()
    {

        
        totalTime = remainingTime;
        mouseFollower.transform.position = pointerHolder[currentMaze].transform.position;
        isInMiniGame = true;
    }
    public void StartIntro() {

        StartCoroutine(StartMinigameRoundWithDelay());
    }

    private IEnumerator StartMinigameRoundWithDelay()
    {
        Cursor.visible = false;
        currentMaze = Random.Range(0, 3);
        try
        {
            limbs[GameManager.Instance.Body.handsHarvested].SetActive(true);
        }
        catch
        {
            limbs[0].SetActive(true);
        }
        mazes[currentMaze].SetActive(true);

        AudioPool.Instance.PlayClip2D(startSFX);
        //transform.GetChild(0).gameObject.SetActive(true);
       // transform.GetChild(2).gameObject.SetActive(true);
        StartCanvas.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        Debug.Log("Starting in 3...");
        yield return new WaitForSeconds(1f);
        StartCanvas.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        StartCanvas.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
        Debug.Log("Starting in 2...");
        yield return new WaitForSeconds(1f);
        Debug.Log("Starting in 1...");
        StartCanvas.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        StartCanvas.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);

        StartGame();
        StartCanvas.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
        Debug.Log("Mini-game started!");

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        Vector3 screenPos = cam.WorldToScreenPoint(target.position);
        Mouse.current.WarpCursorPosition(screenPos);
        FindAnyObjectByType<BoneMouseScript>().enabled = true;
    }


}
