using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class mashingMinigame : MonoBehaviour
{
    public bool isMashing = false;

    public Slider mashBar;
    public KeyCode button0 = KeyCode.M;
    public KeyCode button1 = KeyCode.N;
    public int mashDuration = 100;
    private int buttonPress = 0;
    public GameObject scoop;
    public int tiltAngel1;
    public int tiltAngel2;
    public int smooth;
    public EyeMinigameController emc;

    public void Start()
    {
        mashBar.maxValue = mashDuration;
    }

    public void Update()
    {
        if (isMashing)
        {
            if (Input.GetKeyDown(button0) && buttonPress == 0)
            {
                mashBar.value++;
                buttonPress = 1;
            }
            else if (Input.GetKeyDown(button1) && buttonPress == 1)
            {
                mashBar.value++;
                buttonPress = 0;
            }

            if(buttonPress == 0)
            {
                float tiltAroundZ = Input.GetAxis("Horizontal") * tiltAngel1;
                UnityEngine.Quaternion target = UnityEngine.Quaternion.Euler(0, 0, tiltAroundZ);
                scoop.transform.rotation = UnityEngine.Quaternion.Slerp(scoop.transform.rotation, target, Time.deltaTime * smooth);
            }
            else if (buttonPress == 1)
            {
                float tiltAroundZ = Input.GetAxis("Horizontal") * tiltAngel2;
                UnityEngine.Quaternion target = UnityEngine.Quaternion.Euler(0, 0, tiltAroundZ);
                scoop.transform.rotation = UnityEngine.Quaternion.Slerp(scoop.transform.rotation, target, Time.deltaTime * smooth);
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
