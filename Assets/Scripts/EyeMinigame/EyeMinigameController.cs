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

    public void Awake()
    {
        scoopSliderVertical.value = 0;
        scoopSliderHorizontal.value = 1;
        scoopSliderHorizontal.enabled = false;
    }

    private void FixedUpdate()
    {
        if (minigameRunning)
        {
            if (scoopSliderVertical.enabled && !scoopSliderHorizontal.enabled)
            {
                Debug.Log("running");
                if (scoopSliderVertical.value > 0)
                {
                    scoopSliderVertical.value += 0.1f - Time.deltaTime;
                }
                else if (scoopSliderVertical.value < 0)
                {
                    scoopSliderVertical.value -= 0.1f - Time.deltaTime;
                }
            }
            else if (scoopSliderHorizontal.enabled && !scoopSliderVertical.enabled)
            {
                if (scoopSliderHorizontal.value > 0)
                {
                    scoopSliderHorizontal.value += 0.1f - Time.deltaTime;
                }
                else if (scoopSliderHorizontal.value < 0)
                {
                    scoopSliderHorizontal.value -= 0.1f - Time.deltaTime;
                }
            }
        }
    }
}
