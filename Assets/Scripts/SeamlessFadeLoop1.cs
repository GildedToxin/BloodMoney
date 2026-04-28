using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SeamlessFadeLoop1 : MonoBehaviour
{
    public float fadeInDuration = 2f;
    public float fadeOutDuration = 2f;

    private AudioSource audioSource;

    public float MaxVolume;
    public Coroutine musicCoroutine;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(LoopRoutine());
    }
    public void StartMiniGameMusic()
    {
        musicCoroutine = StartCoroutine(LoopRoutine());
    }
    public void EndMiniGameMusic()
    {
        StopCoroutine(musicCoroutine);
    }
    IEnumerator LoopRoutine()
    {
        while (true)
        {
            // Start clip
            audioSource.volume = 0f;
            audioSource.Play();

            // Fade in
            yield return StartCoroutine(Fade(0f, MaxVolume, fadeInDuration));

            // Wait until it's time to fade out
            float playTime = audioSource.clip.length - fadeOutDuration;
            yield return new WaitForSeconds(playTime - fadeInDuration);

            // Fade out
            yield return StartCoroutine(Fade(MaxVolume, 0f, fadeOutDuration));

            audioSource.Stop();
        }
    }

    IEnumerator Fade(float start, float end, float duration)
    {
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, end, time / duration);
            yield return null;
        }

        audioSource.volume = end;
    }
}