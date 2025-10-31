using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
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


    public int roomNumber = 0;

    public static int[] roomNumbers = new int[]
{
        // Floor 2
        201, 203, 205,
        // Floor 3
        301, 302, 304, 306, 307, 308, 310,
        // Floor 4
        401, 402, 404, 406, 407, 408, 410
};
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
        if (Input.GetKeyDown(KeyCode.V))
        {
            SaveSystem.Save();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            SaveSystem.Load();
        }
    }
    public void newRoom()
    {
        roomNumber = Random.Range(0, roomNumbers.Length);
    }
    public void ProgressDay()
    {
        currentDay += 1;
        /*
        Refresh shop keepers
        */
    }
}
