using UnityEngine;
using UnityEngine.EventSystems;

public class UIHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isHovered;

    //This lowkey should be in its own script but idrc, if we reuse this then ill do that
    public bool isLocked;
    
    public void OnPointerEnter(PointerEventData eventData)
    {

        isHovered = true;
        FindFirstObjectByType<DaySelectManager>().UpdateHover(this);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        FindFirstObjectByType<DaySelectManager>().UpdateHover(this);
    }
}