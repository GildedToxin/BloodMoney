using UnityEngine;
using UnityEngine.Android;
using System.IO;

public class SaveSystem
{
    private static SaveData _saveData = new SaveData();

    [System.Serializable]
    public struct SaveData
    {
        public PlayerSaveData PlayerData;
        public GameManagerSaveData GameManagerSaveData;
    }

    public static string SaveFileName()
    {
        string saveFile = Application.persistentDataPath + "/save" + ".save";
        return saveFile;
    }

    public static void Save()
    {
        HandleSaveData();

        File.WriteAllText(SaveFileName(), JsonUtility.ToJson(_saveData, true));
    }

    private static void HandleSaveData()
    {
        try
        {
            GameManager.Instance.Player.Save(ref _saveData.PlayerData);
        }
        catch { }
        GameManager.Instance.Save(ref _saveData.GameManagerSaveData);
    }
    public static void Load()
    {
        string path = SaveFileName();

        if (!File.Exists(path))
        {
            Debug.Log("No save file found. Creating new save.");
            _saveData = new SaveData(); // default data
            Save();
            return;
        }

        string saveContent = File.ReadAllText(path);
        _saveData = JsonUtility.FromJson<SaveData>(saveContent);

        HandleLoadData();
    }

    private static void HandleLoadData()
    {
        try
        {
            GameManager.Instance.Player.Load(_saveData.PlayerData);
        }
        catch { }
        GameManager.Instance.Load(_saveData.GameManagerSaveData);
    }
}
