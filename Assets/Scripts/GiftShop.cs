using UnityEngine;

public class GiftShop : MonoBehaviour, IPlayerLookTarget
{
    public bool isLookedAt = false;
   
    void Update()
    {
        if(isLookedAt && Input.GetKeyDown(KeyCode.E))
        {
            //FindAnyObjectByType<ShopManager>().gameObject.SetActive(true);
            ShopManager.Instance.transform.GetChild(0).gameObject.SetActive(true);
            FindAnyObjectByType<HUDManager>().UpdateCrossHairText("");

            ShopManager.Instance.RefreshShop(ShopManager.Instance.itemShopContent);
            ShopManager.Instance.RefreshShop(ShopManager.Instance.toolShopContent);
            Camera.main.GetComponent<CameraMovement>().OpenUI();
        }
    }

    public void OnLookEnter()
    {
        FindAnyObjectByType<HUDManager>().CrossHairText.transform.parent.parent.gameObject.SetActive(true);
        FindAnyObjectByType<HUDManager>().UpdateCrossHairText("Press E to open shop");

        isLookedAt = true;
    }
    public void OnLookExit()
    {
        FindAnyObjectByType<HUDManager>().CrossHairText.transform.parent.parent.gameObject.SetActive(false);
        isLookedAt = false;
    }
}
