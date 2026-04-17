using UnityEngine;
using UnityEngine.Rendering;

public class MeatGrinder : MonoBehaviour, IPlayerLookTarget
{
    public bool isLookedAt = false;
    public AudioClip grinderSFX;

    private void Update()
    {
        if (FindFirstObjectByType<HeldItem>().currentItem != null && isLookedAt && Input.GetKeyDown(KeyCode.E)) 
        {
            var item = FindFirstObjectByType<HeldItem>().currentItem;
            Destroy(item);
            FindFirstObjectByType<HeldItem>().DropItem(item);
            FindAnyObjectByType<HUDManager>().meatGrinderUI.gameObject.SetActive(false);
            // add money
            FindAnyObjectByType<InventoryController>().AddMoney(item.GetComponent<OrganManager>().GetOrganPrice() / 2);
            AudioPool.Instance.PlayClip2D(grinderSFX, volume: 0.4f);

        }
    }
    public void OnLookEnter() { 
        isLookedAt = true;

        FindAnyObjectByType<HUDManager>().meatGrinderUI.gameObject.SetActive(true);

        if(FindFirstObjectByType<HeldItem>().currentItem != null)
        {
            FindAnyObjectByType<HUDManager>().meatGrinderUI.SetIcon(FindFirstObjectByType<HeldItem>().currentItem.GetComponent<OrganManager>().organType);
            FindAnyObjectByType<HUDManager>().meatGrinderUI.SetTextSell(FindFirstObjectByType<HeldItem>().currentItem.GetComponent<OrganManager>().organType);

        }
        else
        {

            FindAnyObjectByType<HUDManager>().meatGrinderUI.SetText("You're not holding an organ!");
        }
    }
    public void OnLookExit()
    {
        isLookedAt = false;
        FindAnyObjectByType<HUDManager>().meatGrinderUI.gameObject.SetActive(false);
    }
}
