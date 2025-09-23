using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int money = 20;
    public InventoryUIManager inventoryUIManager;
    public InventoryController inventoryController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            inventoryUIManager.gameObject.SetActive(!inventoryUIManager.gameObject.activeSelf);
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            inventoryController.BuyItem(Resources.Load<Item>("Items/DebugItem"), 10, this);
        }
    }
}
