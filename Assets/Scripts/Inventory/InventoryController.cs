using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public ObservableValue<int> money = new ObservableValue<int>(100);
    //public ObservableArray<Item> inventory = new ObservableArray<Item>(7);
    public ObservableValue<int> selectedIndex = new ObservableValue<int>(-1);

    public Dictionary<string, bool> items = new Dictionary<string, bool>()
          {
        { "Mop", false },
        { "Wood Saw", false },
        { "Jig Saw", false },
        { "Syringe", false },
        { "Mallet", false },
        { "Hatchet", false },
        { "Scooper", false },
    };
    public List<GameObject> tools = new List<GameObject>(); 

    private void Awake()
    {
        for(int i = 0; i <= GameManager.Instance.currentDay; i++)
        {
            try
            {
                print(i);
                var itemCheck = items.ElementAt(i);

                if (itemCheck.Key != null)
                    items[itemCheck.Key] = true;

            }            catch (ArgumentOutOfRangeException)
            {
                break;
            }
        }
        selectedIndex.Value = 1;
    ChangeModel(1);
    }

    private void Update()
    {
        int newSelection = GetNumberKeyDown();

        if (DoesPlayerHaveItemInSlot(newSelection - 1))
        {
            selectedIndex.Value = newSelection != selectedIndex ? newSelection : selectedIndex;
            ChangeModel(selectedIndex.Value);


        }
    }

    public void ChangeModel(int index)
    {
        foreach (GameObject tool in tools)
        {
            if (tool != null)
                tool.SetActive(false);
        }
        tools[index - 1]?.SetActive(true);


    }
    public void HideAllTools()
    {
        foreach (GameObject tool in tools)
        {
            if (tool != null)
                tool.SetActive(false);
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
