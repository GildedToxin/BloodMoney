using JetBrains.Annotations;
using System;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public ObservableArray<Item> inventory = new ObservableArray<Item>(24);

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (!TryAddItem(Resources.Load<Item>("Items/DebugItem")))
            {
                print("Failed to add item to inventory!");
            }
        }
    }
    public bool TryAddItem(Item newItem)
    {
        int emptyIndex = -1;
        foreach (Item item in inventory)
        {
            emptyIndex++;
            if (item == null)
            {
                inventory[emptyIndex] = newItem;
                return true;
            }
        }
        return false;
    }
    public bool TryAddItemAtIndex(Item newItem, int index)
    {
        if (index < 0 || index >= inventory.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
        }
        inventory[index] = newItem;
        return false;
    }
    public bool TryRemoveItemAtIndex(int index)
    {
        if (index < 0 || index >= inventory.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
        }
        inventory[index] = null;
        return false;
    }
    public Item GetItemAtIndex(int index)
    {
        if (index < 0 || index >= inventory.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
        }
        return inventory[index];
    }
}
