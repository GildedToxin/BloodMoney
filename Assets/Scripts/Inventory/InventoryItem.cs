using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    [HideInInspector] public Item item;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        print(item.name);
        InitalizeItem(item);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitalizeItem(Item newItem)
    {
        item = newItem;
        if(item != null)
        {
            print("added to inventory!");
        }
    }
}
