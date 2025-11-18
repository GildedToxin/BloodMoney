using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public GameObject roomPrefab;
    public GameObject CurrentRoom;
    public DoorController CurrentDoor;


    public GameObject clockText; 
    public float realDuration = 1200f; // 20 minutes in seconds

    private float elapsedTime = 0f;
    private float startHour = 0f;  // 12 AM
    private float endHour = 8f;    // 8 AM

    public bool isInHotelRoom;
    public bool isInMiniGame;
    public DeadBody Body;

    // Blood splatter management
    public bool updateBloodSplatters = false;
    public float totalBloodSplatters;
    public float remainingBloodSplatters;
    public float bloodSplatterScore;

    public string currentMiniGame;
    public Camera cam;
    public HUDManager hudManager;
    public static GameManager Instance
    {
        get
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return null;
            }

            if (instance == null)
            {
                Instantiate(Resources.Load<GameManager>("GameManager"));
            }
#endif
            return instance;
        }
    }

    public int currentDay;
    private int maxDay = 30;
    public PlayerController Player { get; set; } // Reference to the player character set in the PlayerController script

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        cam = Camera.main;
        hudManager = FindAnyObjectByType<HUDManager>();
    }

    private void Update()
    {
        UpdateClock();
        if (CurrentDoor != null && CurrentDoor.GetComponentInChildren<Renderer>().isVisible)
        {
            print(true);
        }
        if (Input.GetKeyDown(KeyCode.X)) { 
            if(CurrentRoom != null)
            {
                CurrentDoor.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                Destroy(CurrentRoom);
            }
            if (CurrentDoor != null)
                CurrentDoor.transform.GetChild(0).GetChild(1).gameObject.layer = LayerMask.NameToLayer("Default");

            CurrentDoor = ChooseNewRoom();

            CurrentDoor.transform.GetChild(0).GetChild(1).gameObject.layer = LayerMask.NameToLayer("Highlight");

            //CurrentDoor.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            //CurrentDoor.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            //CurrentRoom = Instantiate(roomPrefab, CurrentDoor.pivot.transform.position, CurrentDoor.pivot.transform.rotation);

            FindAnyObjectByType<HUDManager>().UpdateRoomNumber(CurrentDoor.RoomNumber);
        }

        if( Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.UnloadSceneAsync(currentMiniGame);
            Camera.main.gameObject.SetActive(true);
            FindAnyObjectByType<HUDManager>().gameObject.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            SaveSystem.Save();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            SaveSystem.Load();
        }


        // input for testing blood splatter scoring
        if (Input.GetKeyDown(KeyCode.N))
        {
            StartBloodCalculation();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            ScoreBloodSplatter();
        }

    }
    public DoorController ChooseNewRoom()
    {
        List<DoorController> doors = new List<DoorController>();

        foreach (DoorController door in FindObjectsByType<DoorController>(FindObjectsSortMode.None))
        {
            if(door.canBeOpened)
                doors.Add(door);
        }

        int randomIndex = UnityEngine.Random.Range(0, doors.Count);
        return doors[randomIndex];
    }
    public void ProgressDay()
    {
        currentDay += 1;
        /*
        Refresh shop keepers
        */
    }

    public void UpdateClock()
    {
        elapsedTime += Time.deltaTime;

        // Calculate normalized time (0 to 1)
        float t = Mathf.Clamp01(elapsedTime / realDuration);

        // Convert to in-game hours
        float currentHour = Mathf.Lerp(startHour, endHour, t);

        // Get hours and minutes
        int hour = Mathf.FloorToInt(currentHour);
        int minute = Mathf.FloorToInt((currentHour - hour) * 60f);

        string suffix = "AM";
        int displayHour = hour == 0 ? 12 : hour; // 0 → 12
        clockText.GetComponent<TextMeshProUGUI>().text = $"{displayHour:00}:{minute:00} {suffix}";

        if (t >= 1f)
        {
            Debug.Log("Time's up! 8 AM reached.");
        }

    }
    public string GetOrganFromSlot(int slot)
    {
        Body = FindAnyObjectByType<DeadBody>();
        switch (slot)
        {
            case 0:
                Body.RemoveHighlight();
                return "";

            case 1:
                if (Body.IsLimbsHarvested)
                    break;

                return "Limbs";
            case 2:
                if (Body.IsBonesHarvested)
                    break;

                return "Bones";
            case 3:
                if (Body.IsBloodHarvested)
                    break;

                return "Blood";
            case 4:
                if (Body.IsBrainHarvested)
                    break;

                return "Brain";
            case 5:
                if (Body.IsFingersHarvested)
                    break;

                return "Fingers";
            case 6:
                if (Body.IsEyesHarvested)
                    break;
                return "Eyes";

            default:
                return "";
        }
        return "";
    }

    private Action<int> onSelectedIndexChanged;
    private void OnEnable()
    {
        // Store the lambda in a field
        onSelectedIndexChanged = (i) => Body.Highlight(GetOrganFromSlot(i - 1));
        FindAnyObjectByType<InventoryController>().selectedIndex.OnValueChanged += onSelectedIndexChanged;
    }

    private void OnDisable()
    {
        // Remove the same delegate
        FindAnyObjectByType<InventoryController>().selectedIndex.OnValueChanged -= onSelectedIndexChanged;
    }

    public string OrganToScene(string organ)
    {
        switch (organ)
        {
            case "Limbs":
                return "LimbMiniGame";
            case "Bones":
                return "";
            case "Blood":
                return "";
            case "Brain":
                return "SkullMinigame";
            case "Fingers":
                return "";
            case "Eyes":
                return "";
        }
        return "";
    }
    public void StartMiniGame()
    {
        currentMiniGame = OrganToScene(GetOrganFromSlot(FindAnyObjectByType<InventoryController>().selectedIndex.Value - 1));
        try
        {
            if (currentMiniGame == "")
                return;
            
            SceneManager.LoadScene(currentMiniGame, LoadSceneMode.Additive);
            Camera.main.gameObject.SetActive(false);
            FindAnyObjectByType<HUDManager>().gameObject.SetActive(false);
            isInMiniGame = true;
        }
        catch (Exception e)
        {
            Debug.Log("No minigame for this organ yet.");
        }
    }
    public void StopMiniGame(string sceneName, Camera miniGameCam)
    {
        SceneManager.UnloadSceneAsync(sceneName);
        cam.gameObject.SetActive(true);
        miniGameCam.gameObject.SetActive(false);
        hudManager.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isInMiniGame = false;
        Body.SpawnOrgan(sceneName);
    }


    // Handling blood scoring and calculation
    public void StartBloodCalculation()
    {
        totalBloodSplatters = 0;
        remainingBloodSplatters = 0;
        updateBloodSplatters = true;
        
        // for each object with the tag "Blood" add to totalBloodSplatters and remainingBloodSplatters float
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Blood"))
        {
            totalBloodSplatters += 1;
            remainingBloodSplatters += 1;
        }
    }

    public void ScoreBloodSplatter()
    {
        updateBloodSplatters = false;
        bloodSplatterScore = remainingBloodSplatters / totalBloodSplatters * 100f;
        Debug.Log($"Blood Splatter Score: {bloodSplatterScore}%");
    }

}
