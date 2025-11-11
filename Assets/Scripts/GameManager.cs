using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

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
        if (Input.GetKeyDown(KeyCode.V))
        {
            SaveSystem.Save();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            SaveSystem.Load();
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

        int randomIndex = Random.Range(0, doors.Count);
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
}
