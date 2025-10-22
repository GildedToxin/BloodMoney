using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItem", menuName = "Scriptable Objects/InventoryItem")]
public class Item : ScriptableObject
{
    public string name;
    public string description;
    public int price;
    public Sprite icon;
    public ItemType type;  
}



public enum ItemType
{
tool, 
organ
}
