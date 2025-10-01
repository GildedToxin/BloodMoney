using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerTestFloat;
    public InventoryUIManager inventoryUIManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.Player = this;
    }

    // Update is called once per frame

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            inventoryUIManager.gameObject.SetActive(!inventoryUIManager.gameObject.activeSelf);
        }
    }

    public void Save(ref PlayerSaveData data)
    {
        // Save any player data here by setting variables in PlayerSaveData to current PlayerController Variables
        data.testFloat = playerTestFloat;
    }

    public void Load(PlayerSaveData data)
    {
        // Set variables in the player from the stored player save data here
        playerTestFloat = data.testFloat;
    }
}

[System.Serializable]
public struct PlayerSaveData
{
    // Create additional variables here to store player data
    public float testFloat;
}
