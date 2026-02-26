using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class mashingMinigame : MonoBehaviour
{
    public bool isMashing = false;
    public EyeMinigameController emc;

    [Header("inputs")]
    public KeyCode button0 = KeyCode.M;
    public KeyCode button1 = KeyCode.N;
    private int buttonPress = 0;

    [Header("duration")]
    public Slider mashBar;
    public int mashDuration = 100;

    [Header("animation")]
    public GameObject scoop;
    public int tiltAngel1;
    public int tiltAngel2;
    public int smooth;

    public void Start()
    {
        mashBar.maxValue = mashDuration;
    }

    public void Update()
    {
        if (isMashing)
        {
            if (buttonPress == 0)
            {
                UnityEngine.Quaternion target = UnityEngine.Quaternion.Euler(0, 0, tiltAngel1);
                scoop.transform.rotation = UnityEngine.Quaternion.Slerp(scoop.transform.rotation, target, Time.deltaTime * smooth);

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

                if (Input.GetKeyDown(button1))
                {
                    mashBar.value++;
                    buttonPress = 0;
                }
            }

            if(mashBar.value == mashBar.maxValue)
            {
                buttonPress = 2;
                Debug.Log("win");
                isMashing = false;
                emc.timerStop = true;
            }
        }
    }

}
