using UnityEngine;
using System.Collections.Generic;

public class HeldItem : MonoBehaviour
{
    //Place script onto player and call functions from the inventory scripts

    public List<GameObject> itemPrefabs; // List of possible item prefabs to choose from
    public GameObject currentItemPrefab;
    public Transform heldItemPosition; // Drag and drop the position in the inspector or set it manually

    /// <summary>
    /// Changes the currently held item to the item at the specified index in the itemPrefabs list.
    /// </summary>
    /// <param name="index"></param>
    public void ChangeCurrentItem(int index)
    {
        currentItemPrefab = itemPrefabs[index];
    }

    /// <summary>
    /// Spawns the item selected in currentItemPrefab
    /// </summary>
    public void SpawnHeldItem()
    {
        // Clear previous held item
        foreach (Transform child in heldItemPosition)
        {
            Destroy(child.gameObject);
        }

        // Spawn new held item
        if (currentItemPrefab != null)
        {
            GameObject spawnedObject = Instantiate(currentItemPrefab, heldItemPosition.position, heldItemPosition.rotation);
            spawnedObject.transform.SetParent(heldItemPosition);
        }
    }
}
