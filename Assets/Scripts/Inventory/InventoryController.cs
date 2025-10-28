using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public ObservableValue<int> money = new ObservableValue<int>(100);
    //public ObservableArray<Item> inventory = new ObservableArray<Item>(7);
    public ObservableValue<int> selectedIndex = new ObservableValue<int>(-1);

    public Dictionary<string, bool> items = new Dictionary<string, bool>()
          {
        { "Mop", true },
        { "Syringe", false },
        { "Hatchet", false },
        { "Jig Saw", false },
        { "Wood Saw", false },
        { "Scooper", false },
        { "Mallet", false },
    };


    private void Update()
    {
        selectedIndex.Value = GetNumberKeyDown() != selectedIndex ? GetNumberKeyDown() : selectedIndex;
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
    public void SubtractMoney(int amount)
    {
        money.Value -= amount;
    }
    public bool DoesPlayerHaveItem(string item)
    {
        if (items.TryGetValue(item, out bool hasItem))
        {
            return hasItem;
        }
        return false;
    }
}
