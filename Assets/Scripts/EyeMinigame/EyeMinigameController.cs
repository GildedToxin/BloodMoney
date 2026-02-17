using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.UI;

public class EyeMinigameController : MonoBehaviour
{
    public bool minigameRunning = true;

    [Header("slider variables")]
    public GameObject scoopIcon;
    public Slider scoopSliderVertical;
    public Slider scoopSliderHorizontal;
    private float scoopVerticalPosition = 0f;
    private float scoopHorizontalPosition = 0f;
    public float speed = 0.01f;
    private float SliderMovement = 0f;

    public void Awake()
    {
        scoopSliderVertical.value = 0;
        scoopSliderHorizontal.value = 0;
        scoopSliderHorizontal.enabled = false;
        SliderMovement = speed * Time.deltaTime;
    }

    private void Update()
    {
        if (minigameRunning)
        {
            VerticalMovement();
        }
    }

    void VerticalMovement()
    {
        bool fill = true;
        if (fill)
        {
            scoopSliderVertical.value = scoopSliderVertical.value + SliderMovement;
        }
        else if (!fill)
        {
            scoopSliderVertical.value = scoopSliderVertical.value - SliderMovement;
        }

        if (scoopSliderVertical.value == 1)
        {
            fill = false;
        }
        else if (scoopSliderVertical.value == 0)
        {
            fill = true;
        }
    }
}
