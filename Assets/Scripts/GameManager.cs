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
    public float realDuration = 300f; // 20 minutes in seconds

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

    public bool doesPlayerHaveKey = false;
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

    public int currentDay = 0;
    private const int MAXDAY = 10;
    public int moneyMadeToday;
    public PlayerController Player { get; set; } // Reference to the player character set in the PlayerController script



    public Dictionary<string, bool> HasShownTutorial = new Dictionary<string, bool>()
    {
        { "LimbMiniGame", false },
        { "BloodMiniGame", false },
        { "SkullMinigame", false },
        { "HandsMinigame", false },
        { "EyeMinigame", false },
        { "BoneMinigame", false },
    };

    public int[] quota;

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
    private void Start()
    {

        quota = new int[MAXDAY] { 1000, 1500, 2000, 2500, 3000, 3500, 4000, 4500, 5000, 5500 };
        if (currentDay == 0)
        {
            FirstDayTutorial();
        }
    }
    private void Update()
    {
        if(moneyMadeToday >= CalculateQuota())
        {
            EndDay();
        }
        UpdateClock();
        if (Input.GetKeyDown(KeyCode.X)) {
            GivePlayerKey();
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
    public void GivePlayerKey()
    {
        if (CurrentRoom != null)
        {
            CurrentDoor.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            Destroy(CurrentRoom);
        }
        if(CurrentDoor != null)
        {
            CurrentDoor.gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            CurrentDoor.gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
            CurrentDoor.gameObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
            CurrentDoor.isOpened = false;
        }
        if (CurrentDoor != null)
            CurrentDoor.transform.GetChild(0).GetChild(1).gameObject.layer = LayerMask.NameToLayer("Default");

        CurrentDoor = ChooseNewRoom();

        CurrentDoor.transform.GetChild(0).GetChild(1).gameObject.layer = LayerMask.NameToLayer("Highlight");

        CurrentRoom = Instantiate(roomPrefab, CurrentDoor.pivot.transform.position, CurrentDoor.pivot.transform.rotation);
        Body = CurrentRoom.GetComponent<RoomManager>().body;
        Body.Highlight(GetOrganFromSlot(FindAnyObjectByType<InventoryController>().selectedIndex.Value));

        FindAnyObjectByType<HUDManager>().UpdateRoomNumber(CurrentDoor.RoomNumber);
        FindAnyObjectByType<VendorStand>().RefreshCustomers();
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
        moneyMadeToday = 0;
        /*
        Refresh shop keepers
        */
    }
    public void EndDay()
    {

    }
    int lastMinute = -1;  
    public void UpdateClock()
    {
        elapsedTime += Time.deltaTime;

        float t = Mathf.Clamp01(elapsedTime / realDuration);

        float currentHour = Mathf.Lerp(startHour, endHour, t);

        int hour = Mathf.FloorToInt(currentHour);
        int minute = Mathf.FloorToInt((currentHour - hour) * 60f);

        // Only update at :00, :15, :30, :45
        if ((minute % 15 == 0) && minute != lastMinute)
        {
            lastMinute = minute;
            string suffix = "AM";
            int displayHour = hour == 0 ? 12 : hour;
            clockText.GetComponent<TextMeshProUGUI>().text = $"{displayHour:00}:{minute:00} {suffix}";
        }

        if (t >= 1f)
        {
            PlayerFailedDay();
        }
    }
    public string GetOrganFromSlot(int slot)
    {
            Body = FindAnyObjectByType<DeadBody>();
        if (Body == null)
            return "";
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
        onSelectedIndexChanged = (i) =>
        {
            var highlight = GetOrganFromSlot(i - 1);
            if (highlight != "")
            {
                Body.Highlight(highlight);
            }
        };
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
                return "BloodMiniGame";
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
            Player.GetComponent<PlayerController>().enabled = false;
            FindAnyObjectByType<HUDManager>().gameObject.SetActive(false);
            isInMiniGame = true;

            if(currentMiniGame == "BloodMiniGame")
            {
                
                FindAnyObjectByType<BloodMinigameScript>().UpdateDeadBodyModel(HandsHarvested: Body.handsHarvested, LimbsHavested: Body.limbsHarvested, Hands: Body.IsFingersHarvested, 
                    Limbs: Body.IsLimbsHarvested, Skull: Body.IsBrainHarvested, Ribs: Body.IsBonesHarvested);
            }

        }
        catch (Exception e)
        {
            Debug.Log("No minigame for this organ yet.");
        }
    }
    public void StopMiniGame(string sceneName, Camera miniGameCam)
    {
        Player.GetComponent<PlayerController>().enabled = true;
        SceneManager.UnloadSceneAsync(sceneName);
        cam.gameObject.SetActive(true);
        miniGameCam.gameObject.SetActive(false);
        hudManager.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isInMiniGame = false;
        //Body.SpawnOrgan(sceneName);
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

    public void FirstDayTutorial()
    {
        // show UI message for first day tutorial

        FindAnyObjectByType<owner>().transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Highlight");
        //currentDay++;
    }

    public int CalculateQuota()
    {
        // For now this returns a fixed quota for the first day
        // Later this can be expanded to have dynamic quotas based on day and difficulty

        return quota[currentDay];
    }
    public bool HasPlayerHitQuota()
    {
        return moneyMadeToday >= CalculateQuota();
    }

    public void PlayerFailedDay()
    {
        FindAnyObjectByType<EndGameCanvas>(FindObjectsInactive.Include).gameObject.SetActive(true);
        FindAnyObjectByType<EndGameCanvas>(FindObjectsInactive.Include).lostGame.SetActive(true);
        FindAnyObjectByType<EndGameCanvas>(FindObjectsInactive.Include).wonGame.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        FindAnyObjectByType<PlayerController>().enabled = false;
    }


    public MiniGameType testMiniGame;

    [ContextMenu("Start Test MiniGame")]
    public void StartTestMiniGame()
    {
        switch (testMiniGame)
        {
            case MiniGameType.Blood:
                SceneManager.LoadScene("BloodMiniGame", LoadSceneMode.Additive);
                break;
            case MiniGameType.Limb:
                SceneManager.LoadScene("LimbMiniGame", LoadSceneMode.Additive);
                break;
            case MiniGameType.Brain:
                SceneManager.LoadScene("SkullMinigame", LoadSceneMode.Additive);
                break;
        }

        Camera.main.gameObject.SetActive(false);
        Player.GetComponent<PlayerController>().enabled = false;
        FindAnyObjectByType<HUDManager>().gameObject.SetActive(false);
        isInMiniGame = true;

        if (currentMiniGame == "BloodMiniGame")
        {

            FindAnyObjectByType<BloodMinigameScript>().UpdateDeadBodyModel(HandsHarvested: Body.handsHarvested, LimbsHavested: Body.limbsHarvested, Hands: Body.IsFingersHarvested,
                Limbs: Body.IsLimbsHarvested, Skull: Body.IsBrainHarvested, Ribs: Body.IsBonesHarvested);
        }

    }
    [ContextMenu("Stop Test MiniGame")]
    public void StopTestMiniGame()
    {
        switch (testMiniGame)
        {
            case MiniGameType.Blood:
                SceneManager.UnloadSceneAsync("BloodMiniGame");
                break;
            case MiniGameType.Limb:
                SceneManager.UnloadSceneAsync("LimbMiniGame");
                break;
            case MiniGameType.Brain:
                SceneManager.UnloadSceneAsync("SkullMinigame");
                break;
        }

        Player.GetComponent<PlayerController>().enabled = true;
        cam.gameObject.SetActive(true);
        hudManager.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isInMiniGame = false;
        //Body.SpawnOrgan(sceneName);
    }
}
public enum MiniGameType
{
    Blood,
    Limb,
    Brain
}

