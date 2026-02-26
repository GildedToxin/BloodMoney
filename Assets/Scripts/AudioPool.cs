using System.Collections.Generic;
using UnityEngine;

public class AudioPool : MonoBehaviour
{
    public static AudioPool Instance { get; private set; }

    [SerializeField] private int poolSize = 10;
    [SerializeField] private GameObject audioSourcePrefab;

    private Queue<AudioSource> availableSources = new Queue<AudioSource>();
    private List<AudioSource> allSources = new List<AudioSource>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializePool();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            CreateNewAudioSource();
        }
    }

    private AudioSource CreateNewAudioSource()
    {
        GameObject obj;

        if (audioSourcePrefab != null)
        {
            obj = Instantiate(audioSourcePrefab, transform);
        }
        else
        {
            obj = new GameObject($"PooledAudioSource_{allSources.Count}");
            obj.transform.SetParent(transform);
            obj.AddComponent<AudioSource>();
        }

        AudioSource source = obj.GetComponent<AudioSource>();
        source.playOnAwake = false;

        allSources.Add(source);
        availableSources.Enqueue(source);

        return source;
    }

    public AudioSource GetAudioSource()
    {
        if (availableSources.Count == 0)
        {
            CreateNewAudioSource();
        }

        return availableSources.Dequeue();
    }

    public void ReturnAudioSource(AudioSource source)
    {
        if (source != null && !availableSources.Contains(source))
        {
            source.Stop();
            source.clip = null;
            availableSources.Enqueue(source);
        }
    }

    private void Update()
    {
        foreach (var source in allSources)
        {
            if (!source.isPlaying && source.clip != null && !availableSources.Contains(source))
            {
                ReturnAudioSource(source);
            }
        }
    }

    public void PlayClipAtPoint(AudioClip clip, Vector3 position, float volume = 1f, float pitch = 1f)
    {
        if (clip == null) return;

        AudioSource source = GetAudioSource();
        source.transform.position = position;
        source.clip = clip;
        source.volume = volume;
        source.spatialBlend = 1f;
        source.pitch = pitch;
        source.Play();
    }

    public void PlayClip2D(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        if (clip == null) return;

        AudioSource source = GetAudioSource();
        source.clip = clip;
        source.volume = volume;
        source.spatialBlend = 0f;
        source.pitch = pitch;
        source.Play();
    }
}
