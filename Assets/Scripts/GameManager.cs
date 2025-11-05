using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public GameObject roomPrefab;
    public GameObject CurrentRoom;
    public DoorController CurrentDoor;
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
        if (Input.GetKeyDown(KeyCode.X)) { 
            if(CurrentRoom != null)
            {
                CurrentDoor.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                Destroy(CurrentRoom);
            }
            CurrentDoor = ChooseNewRoom();
            CurrentDoor.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            CurrentRoom = Instantiate(roomPrefab, CurrentDoor.pivot.transform.position, CurrentDoor.pivot.transform.rotation);

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
}
