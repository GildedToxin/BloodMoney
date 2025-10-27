using JetBrains.Annotations;
using System;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public ObservableValue<int> money = new ObservableValue<int>(100);
    public ObservableArray<Item> inventory = new ObservableArray<Item>(5);
    public ObservableValue<int> selectedIndex = new ObservableValue<int>(-1);

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (!TryAddItem(Resources.Load<Item>("Items/DebugItem")))
            {
                print("Failed to add item to inventory!");
            }
        }
        selectedIndex.Value = GetNumberKeyDown() != selectedIndex ? GetNumberKeyDown() : selectedIndex;

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
    private int GetNumberKeyDown()
    {
        for (int i = 0; i <= 9; i++)
        {
            if (Input.GetKey((KeyCode)((int)KeyCode.Alpha0 + i)))
                return i;
        }
        return -1;
    }

    public void AddMoney(int amount)
    {
        money.Value += amount;
    }
}
