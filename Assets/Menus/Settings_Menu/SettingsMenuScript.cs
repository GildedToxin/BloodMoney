using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.UI;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class SettingsMenuScript : MonoBehaviour
{
    public AudioMixer audioMixer;
    public TMP_Dropdown resolutionDropdown;
    public GameObject previousMenu;

    Resolution[] resolutions;

    public Slider volumeSlider;
    public Slider sensitivitySlider;
    public Toggle fullScreenButton;

    public AudioPool audiopool;
    public AudioClip buttonHover;
    public AudioClip Click;

    public PlayerController playerController;

    private int currentResolutionIndex;

    private bool Fullscreen;
    int fullScreenSetting;

    void Start()
    {
        fullScreenSetting = PlayerPrefs.GetInt("IsFullscreen", 1);

        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 100);
        sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity", 10);
        Screen.SetResolution(PlayerPrefs.GetInt("ResolutionWidth", 1920), PlayerPrefs.GetInt("ResolutionHeight", 1080), PlayerPrefs.GetInt("IsFullscreen", 1) == fullScreenSetting);

        if (PlayerPrefs.GetInt("IsFullscreen", 1) == 1)
        {
            fullScreenButton.isOn = true;
        }
        else if(PlayerPrefs.GetInt("IsFullscreen", 1) == 0)
        {
            fullScreenButton.isOn = false;
        }

        previousMenu.SetActive(false);

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == PlayerPrefs.GetInt("ResolutionWidth", 1920) && resolutions[i].height == PlayerPrefs.GetInt("ResolutionHeight", 1080))
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
        if (isFullScreen == true)
        {
            fullScreenSetting = 1;
        }
        else if (isFullScreen == false)
        {
            fullScreenSetting = 0;
        }
        onFullScreenChange(isFullScreen);
        GameManager.Instance.PlayUIButtonPress();
    }

    public void pickResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        onResolutionChange(resolution.width, resolution.height);
        Screen.SetResolution(resolution.width, resolution.height, PlayerPrefs.GetInt("IsFullscreen", 1) == fullScreenSetting);
    }

    public void ButtonHover()
    {
        GameManager.Instance.ButtonHover();
    }

    public void Reset()
    {
        PlayerPrefs.DeleteAll();
        fullScreenSetting = 1;
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 100);
        sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity", 10);
        Screen.SetResolution(PlayerPrefs.GetInt("ResolutionWidth", 1920), PlayerPrefs.GetInt("ResolutionHeight", 1080), PlayerPrefs.GetInt("IsFullscreen", 1) == fullScreenSetting);
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == PlayerPrefs.GetInt("ResolutionWidth", 1920) && resolutions[i].height == PlayerPrefs.GetInt("ResolutionHeight", 1080))
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        if (PlayerPrefs.GetInt("IsFullscreen", 1) == 1)
        {
            fullScreenButton.isOn = true;
        }
        else if (PlayerPrefs.GetInt("IsFullscreen", 1) == 0)
        {
            fullScreenButton.isOn = false;
        }

        PlayerPrefs.Save();
    }

    private void onVolumeChange(float volume)
    {
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();
    }

    private void onSensitivityChange(float sensitivity)
    {
        PlayerPrefs.SetFloat("Sensitivity", sensitivity);
        PlayerPrefs.Save();
    }

    private void onFullScreenChange(bool isFullScreen)
    {
        PlayerPrefs.SetInt("IsFullscreen", Fullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }
    private void onResolutionChange(int width, int height)
    {
        PlayerPrefs.SetInt("ResolutionWidth", width);
        PlayerPrefs.SetInt("ResolutionHeight", height);
        PlayerPrefs.Save();
    }
}
