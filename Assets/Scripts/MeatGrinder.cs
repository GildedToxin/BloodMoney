using UnityEngine;
using UnityEngine.Rendering;

public class MeatGrinder : MonoBehaviour, IPlayerLookTarget
{
    public bool isLookedAt = false;

    private void Update()
    {
        if (FindFirstObjectByType<HeldItem>().currentItem != null && isLookedAt && Input.GetKeyDown(KeyCode.E)) 
        {
            var item = FindFirstObjectByType<HeldItem>().currentItem;
            Destroy(item);
            FindFirstObjectByType<HeldItem>().DropItem(item);
            
        }
    }
    public void OnLookEnter() { 
        isLookedAt = true;
    }
    public void OnLookExit()
    {
        isLookedAt = false;
    }
}
