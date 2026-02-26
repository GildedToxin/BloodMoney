using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class mashingMinigame : MonoBehaviour
{
    public bool isMashing = false;
    public EyeMinigameController emc;

    [Header("inputs")]
    public KeyCode button0 = KeyCode.D;
    public KeyCode button1 = KeyCode.A;
    private int buttonPress = 0;

    [Header("duration")]
    public Slider mashBar;
    public int mashDuration = 100;

    [Header("animation")]
    public GameObject scoop;
    public int tiltAngel1;
    public int tiltAngel2;
    public int smooth;
    public GameObject button0Sprite;
    public GameObject button1Sprite;
    public GameObject button0SpritePress;
    public GameObject button1SpritePress;

    public void Start()
    {
        mashBar.maxValue = mashDuration;
    }

    public void Update()
    {
        if (isMashing)
        {
            //Rotates the object twards the button direction needed, then swaps to the other button when pressed
            if (buttonPress == 0)
            {
                UnityEngine.Quaternion target = UnityEngine.Quaternion.Euler(0, 0, tiltAngel1);
                scoop.transform.rotation = UnityEngine.Quaternion.Slerp(scoop.transform.rotation, target, Time.deltaTime * smooth);
                button0Sprite.SetActive(false);
                button1Sprite.SetActive(true);
                button0SpritePress.SetActive(true);
                button1SpritePress.SetActive(false);

                if (Input.GetKeyDown(button0))
                {
                    mashBar.value++;
                    buttonPress = 1;
                }
            }

            else if (buttonPress == 1)
            {
                UnityEngine.Quaternion target = UnityEngine.Quaternion.Euler(0, 0, tiltAngel2);
                scoop.transform.rotation = UnityEngine.Quaternion.Slerp(scoop.transform.rotation, target, Time.deltaTime * smooth);
                button0Sprite.SetActive(true);
                button1Sprite.SetActive(false);
                button0SpritePress.SetActive(false);
                button1SpritePress.SetActive(true);

                if (Input.GetKeyDown(button1))
                {
                    mashBar.value++;
                    buttonPress = 0;
                }
            }
            
            //runs when the mash bar is maxed
            if(mashBar.value == mashBar.maxValue)
            {
                buttonPress = 2;
                isMashing = false;
                emc.timerStop = true;
                emc.winGame();
            }
        }
    }

}
