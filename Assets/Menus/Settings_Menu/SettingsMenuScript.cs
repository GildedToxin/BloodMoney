using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenuScript : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void setVolume (float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }
}
