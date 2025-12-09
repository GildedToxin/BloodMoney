using UnityEngine;

public class GiftShop : MonoBehaviour, IPlayerLookTarget
{
    public bool isLookedAt = false;
   
    void Update()
    {
        if(isLookedAt && Input.GetKeyDown(KeyCode.Q))
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
        FindAnyObjectByType<HUDManager>().CrossHairText.SetActive(true);
        FindAnyObjectByType<HUDManager>().UpdateCrossHairText("Press Q to Open Shop");

        isLookedAt = true;
    }
    public void OnLookExit()
    {
        FindAnyObjectByType<HUDManager>().CrossHairText.SetActive(false);
        isLookedAt = false;
    }
}
