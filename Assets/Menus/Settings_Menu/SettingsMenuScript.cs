using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.UI;
using NUnit.Framework;
using System.Collections.Generic;
using TMPro;

public class SettingsMenuScript : MonoBehaviour
{
    public AudioMixer audioMixer;
    public TMP_Dropdown resolutionDropdown;
    public GameObject previousMenu;

    Resolution[] resolutions;

    public Slider volumeSlider;
    public Slider sensitivitySlider;
    public Button fullScreenButton;

    public AudioPool audiopool;
    public AudioClip buttonHover;
    public AudioClip Click;

    public PlayerController playerController;

    private int currentResolutionIndex;

    void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 100);
        sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity", 10);
        currentResolutionIndex = PlayerPrefs.GetInt("Resolution", 0);   


        previousMenu.SetActive(false);

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void setVolume (float volume)
    {
        volumeSlider.value = volume;
        if (volume == 0)
        {
            volume = 0.01f;
        }
        volumeSlider.onValueChanged.AddListener(onVolumeChange);
        audioMixer.SetFloat("Volume", Mathf.Log10(volume / 100) * 20f);
    }

    public void setSensitivity(float sensitivity)
    {
        if (sensitivity == 0)
        {
            sensitivity = 0.01f;
        }
        sensitivitySlider.onValueChanged.AddListener(onSensitivityChange);
        playerController.sensitivityX = sensitivity;
        playerController.sensitivityY = sensitivity;
    }

    public void backButton ()
    {
        this.gameObject.SetActive(false);
        GameManager.Instance.PlayUIButtonPress();
        previousMenu.SetActive(true);
    }

    public void fullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        GameManager.Instance.PlayUIButtonPress();
    }

    public void setResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        onResolutionChange(resolutionIndex);
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void ButtonHover()
    {
        GameManager.Instance.ButtonHover();
    }

    public void Reset()
    {
        PlayerPrefs.DeleteAll();
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 100);
        sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity", 10);
        currentResolutionIndex = PlayerPrefs.GetInt("Resolution", 0);
    }

    private void onVolumeChange(float volume)
    {
        PlayerPrefs.SetFloat("Volume", volume);
    }

    private void onSensitivityChange(float sensitivity)
    {
        PlayerPrefs.SetFloat("Sensitivity", sensitivity);
    }

    private void onResolutionChange(int currentResolutionIndex)
    {
        PlayerPrefs.SetInt("Resolution", currentResolutionIndex);
    }
}
