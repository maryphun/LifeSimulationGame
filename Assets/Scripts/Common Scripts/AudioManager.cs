using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
                if (instance == null)
                {
                    instance = new GameObject("Spawned AudioManager", typeof(AudioManager)).GetComponent<AudioManager>();
                }
            }

            return instance;
        }

        private set
        {
            instance = value;
        }
    }

    // create two music source so we can crossfade music to another smoothly
    private AudioSource musicSource;
    private AudioSource musicSource2;
    private AudioSource sfxSource;

    private float musicVolume;

    private bool firstMusicSourceIsPlaying;

    // loaded resources
    [SerializeField]
    private AudioClip[] seClips;
    [SerializeField]
    private AudioClip[] bgmClips;

    private void Awake()
    {
        // make sure we don't destroy this instance
        DontDestroyOnLoad(this.gameObject);

        // create audio source, and save them as references
        musicSource = this.gameObject.AddComponent<AudioSource>();
        musicSource2 = this.gameObject.AddComponent<AudioSource>();
        sfxSource = this.gameObject.AddComponent<AudioSource>();

        // Loop the music tracks
        musicSource.loop = true;
        musicSource2.loop = true;

        // default music volume
        musicVolume = 1f;

        // Load all audio Resources
        seClips = Resources.LoadAll<AudioClip>("Audio/SE");
        bgmClips = Resources.LoadAll<AudioClip>("Audio/BGM");
    }

    public void PlayMusic(string bgmname)
    {
        // Determine which music source is active
        AudioSource activeSource = (firstMusicSourceIsPlaying) ? musicSource : musicSource2;

        AudioClip clipToPlay = NameToBGMClip(bgmname);

        if (clipToPlay != null)
        {
            activeSource.clip = clipToPlay;
            activeSource.volume = musicVolume;
            activeSource.Play();
            return;
        }
    }

    public void PlayMusicWithFade(string bgmname, float transitionTime)
    {
        // Determine which music source is active
        AudioSource activeSource = (firstMusicSourceIsPlaying) ? musicSource : musicSource2;

        AudioClip clipToPlay = NameToBGMClip(bgmname);

        if (clipToPlay != null)
        {
            StartCoroutine(UpdateMusicWithFade(activeSource, clipToPlay, transitionTime));
        }
    }

    public void PlayMusicWithCrossFade(string bgmname, float transitionTime = 1.0f)
    {
        // Determine which music source is active
        AudioSource activeSource = (firstMusicSourceIsPlaying) ? musicSource : musicSource2;
        AudioSource newSource = (firstMusicSourceIsPlaying) ? musicSource2 : musicSource;

        AudioClip clipToPlay = NameToBGMClip(bgmname);

        if (clipToPlay != null)
        {
            // Swap the source
            firstMusicSourceIsPlaying = !firstMusicSourceIsPlaying;

            // Set the fields of the audio source, then start the coroutine to crossfade
            newSource.clip = clipToPlay;
            newSource.Play();
            StartCoroutine(UpdateMusicWithCrossFade(activeSource, newSource, transitionTime));
        }
    }

    private IEnumerator UpdateMusicWithFade(AudioSource activeSource, AudioClip newClip, float transitionTime)
    {
        // make sure the source is active and playing
        if (!activeSource.isPlaying)
            activeSource.Play();

        float t = 0.0f;

        //Fadeout
        for (t = 0; t < transitionTime; t += Time.deltaTime)
        {
            activeSource.volume = (musicVolume - ((t / transitionTime) * musicVolume));
            yield return null;
        }

        activeSource.Stop();
        activeSource.clip = newClip;
        activeSource.Play();

        // Fadein
        for (t = 0; t < transitionTime; t += Time.deltaTime)
        {
            activeSource.volume = ((t / transitionTime) * musicVolume);
            yield return null;
        }
    }

    private IEnumerator UpdateMusicWithCrossFade(AudioSource originalSource, AudioSource newSource, float transitionTime)
    {
        float t = 0.0f;

        for (t = 0; t < transitionTime; t += Time.deltaTime)
        {
            originalSource.volume = (musicVolume - ((t / transitionTime) * musicVolume));
            newSource.volume = ((t / transitionTime) * musicVolume);
            yield return null;
        }

        originalSource.Stop();
    }

    public void PlaySFX(string clipname)
    {
        AudioClip clipToPlay = NameToSEClip(clipname);

        if (clipToPlay != null)
        {
            sfxSource.PlayOneShot(clipToPlay);
        }
    }

    public void PlaySFX(string sename, float volume)
    {
        AudioClip clipToPlay = NameToSEClip(sename);

        if (clipToPlay != null)
        {
            sfxSource.PlayOneShot(clipToPlay, volume);
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
    }

    private AudioClip NameToSEClip(string name)
    {
        AudioClip foundClip = null;
        for (int i = 0; i < seClips.Length; ++i)
        {
            if (seClips[i].name == name)
            {
                foundClip = seClips[i];
            }
        }

        if (foundClip == null)
        {
            Debug.LogWarning("Sound Effect [" + name + "] not found.");
        }

        return foundClip;
    }

    private AudioClip NameToBGMClip(string name)
    {
        AudioClip foundClip = null;
        for (int i = 0; i < bgmClips.Length; ++i)
        {
            if (bgmClips[i].name == name)
            {
                foundClip = bgmClips[i];
            }
        }

        if (foundClip == null)
        {
            Debug.LogWarning("Background Music [" + name + "] not found.");
        }

        return foundClip;
    }
}