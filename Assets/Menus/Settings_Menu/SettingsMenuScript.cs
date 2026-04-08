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
    public GameObject PauseMenu;

    Resolution[] resolutions;

    public AudioPool audiopool;
    public AudioClip buttonHover;
    public AudioClip Click;
    void Start()
    {
        PauseMenu.SetActive(false);

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
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
        if (volume == 0)
        {
            volume = 0.01f;
        }
        audioMixer.SetFloat("Volume", Mathf.Log10(volume / 100) * 20f);
    }

    public void backButton ()
    {
        this.gameObject.SetActive(false);
        audiopool.PlayClip2D(Click);
        PauseMenu.SetActive(true);
    }

    public void fullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        audiopool.PlayClip2D(Click);
    }

    public void setResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void ButtonHover()
    {
        audiopool.PlayClip2D(buttonHover);
    }
}
