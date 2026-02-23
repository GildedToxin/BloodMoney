using UnityEngine;
using System.Collections;

public class FirstDayManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject[] tutorialScreens;

    public int currentScreen = 0;

    public bool isShowingScreen = true;

    public bool canMoveScreen = true;
    void Start()
    {
        if(GameManager.Instance.currentDay != 0)
            this.gameObject.SetActive(false);
        else
            tutorialScreens[currentScreen].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentScreen == 0 && Input.anyKeyDown && canMoveScreen)
        {
            tutorialScreens[currentScreen].SetActive(false);
            currentScreen++; //1
            tutorialScreens[currentScreen].SetActive(true);
            StartCoroutine(WaitForNextScreen());
        }
        else if ((currentScreen == 1 || currentScreen == 2) && Input.GetKeyDown(KeyCode.Tab) && canMoveScreen)
        {
            tutorialScreens[currentScreen].SetActive(false);
            currentScreen++; //2 or 3
            tutorialScreens[currentScreen].SetActive(true);
            StartCoroutine(WaitForNextScreen());
        }
        else if(currentScreen == 3 && Input.anyKeyDown && canMoveScreen)
        {
            tutorialScreens[currentScreen].SetActive(false);
            isShowingScreen = false;
            // 3
            StartCoroutine(WaitForNextScreen());
        }
        else if (currentScreen == 4 && Input.anyKeyDown && canMoveScreen)
        {
            tutorialScreens[currentScreen].SetActive(false);
            currentScreen++; //5
            tutorialScreens[currentScreen].SetActive(true);
            StartCoroutine(WaitForNextScreen());
        }
        else if (currentScreen == 5 && Input.anyKeyDown && canMoveScreen)
        {
            tutorialScreens[currentScreen].SetActive(false);
            isShowingScreen = false;
            // 5
        }
        else if (currentScreen == 6 && Input.anyKeyDown && canMoveScreen)
        {
            tutorialScreens[currentScreen].SetActive(false);
            isShowingScreen = false;
            // 6
       }
        else if (currentScreen == 7 && Input.anyKeyDown && canMoveScreen)
        {
            tutorialScreens[currentScreen].SetActive(false);
            isShowingScreen = false;
            // 7
        }
        else if (currentScreen == 8 && Input.anyKeyDown && canMoveScreen)
        {
            tutorialScreens[currentScreen].SetActive(false);
            isShowingScreen = false;
            // 8
        }
        else if (currentScreen == 9 && Input.GetKeyDown(KeyCode.Alpha2) && canMoveScreen)
        {
            tutorialScreens[currentScreen].SetActive(false);
            currentScreen++; //10
            tutorialScreens[currentScreen].SetActive(true);
            StartCoroutine(WaitForNextScreen());
        }
        else if (currentScreen == 10 && Input.anyKeyDown && canMoveScreen)
        {
            tutorialScreens[currentScreen].SetActive(false);
            isShowingScreen = false;
            // 10
        }
        else if (currentScreen == 11 && Input.anyKeyDown && canMoveScreen)
        {
            tutorialScreens[currentScreen].SetActive(false);
            isShowingScreen = false;
            // 11
        }
        else if (currentScreen == 12 && Input.anyKeyDown && canMoveScreen)
        {
            tutorialScreens[currentScreen].SetActive(false);
            isShowingScreen = false;
            // 12
        }
        else if (currentScreen == 13 && Input.anyKeyDown && canMoveScreen)
        {
            tutorialScreens[currentScreen].SetActive(false);
            currentScreen++; //14
            tutorialScreens[currentScreen].SetActive(true);
            StartCoroutine(WaitForNextScreen());
        }
        else if (currentScreen == 14 && Input.anyKeyDown && canMoveScreen)
        {
            tutorialScreens[currentScreen].SetActive(false);
            isShowingScreen = false;
            //14
        }
        else if (currentScreen == 15 && Input.anyKeyDown && canMoveScreen)
        {
            tutorialScreens[currentScreen].SetActive(false);
            isShowingScreen = false;
            //15
        }
    }

    public IEnumerator WaitForNextScreen()
    {
        canMoveScreen = false;
        yield return new WaitForSeconds(1f);
        canMoveScreen = true;
        yield return null;
    }
}
