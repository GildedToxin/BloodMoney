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
        { "Wood Saw", true },
        { "Scooper", false },
        { "Mallet", false },
    };


    private void Update()
    {
        int newSelection = GetNumberKeyDown();

        if (DoesPlayerHaveItemInSlot(newSelection - 1))
        {
            selectedIndex.Value = newSelection != selectedIndex ? newSelection : selectedIndex;
        }
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
    private bool DoesPlayerHaveItemInSlot(int slot)
    {
        switch (slot)
        {
            case 0:
                return DoesPlayerHaveItem("Mop");
            case 1:
                return DoesPlayerHaveItem("Wood Saw");
            case 2:
                return DoesPlayerHaveItem("Jig Saw");
            case 3:
                return DoesPlayerHaveItem("Syringe");
            case 4:
                return DoesPlayerHaveItem("Mallet");
            case 5:
                return DoesPlayerHaveItem("Hatchet");
            case 6:
                return DoesPlayerHaveItem("Scooper");
            default:
                return false;
        }
    }

    public void AddMoney(int amount)
    {
        GameManager.Instance.moneyMadeToday += amount;
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
