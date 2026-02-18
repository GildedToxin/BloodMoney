using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class EyeMinigameController : MonoBehaviour
{
    public bool minigameRunning = true;

    [Header("slider variables")]
    public GameObject scoopIcon;
    public Slider scoopSliderVertical;
    public Slider scoopSliderHorizontal;
    public GameObject VerticalHandel;
    public GameObject HorizontalHandel;

    private float scoopVerticalPosition = 0f;
    private float scoopHorizontalPosition = 0f;
    public float speed = 0.01f;
    private float SliderMovement = 0f;

    public bool fill = false;

    public void Awake()
    {
        scoopSliderVertical.value = 0;
        scoopSliderHorizontal.value = 0;
        scoopSliderHorizontal.enabled = false;
        SliderMovement = speed * Time.deltaTime;
    }

    private void Update()
    {
        float sliderPosition = scoopSliderVertical.value;
        bool swapSlider = false;

        if (minigameRunning)
        {
            if (sliderPosition == 0 || fill)
            {
                sliderFill(scoopSliderVertical);
                fill = true;
            }

            if (sliderPosition == 1 || !fill)
            {
                sliderEmpty(scoopSliderVertical);
                fill = false;
            }
        }

        Input.GetKeyDown(KeyCode.Space);
        {
            if (!swapSlider)
            {
                scoopVerticalPosition = scoopIcon.transform.position.y;
            }
            else if (swapSlider)
            {
                scoopHorizontalPosition = scoopIcon.transform.position.x;
            }
        }
    }

    

    void sliderFill(Slider slider)
    {
        slider.value = slider.value + SliderMovement;
        scoopIcon.transform.position = new Vector3(scoopIcon.transform.position.x, VerticalHandel.transform.position.y, scoopIcon.transform.position.z);
    }

    void sliderEmpty(Slider slider)
    {
        slider.value = slider.value + (SliderMovement * -1);
    }
}
