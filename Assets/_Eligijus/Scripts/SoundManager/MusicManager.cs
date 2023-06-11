using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance { get; private set; }
    [SerializeField]
    private List<MusicLevel> _musicLevels;
    [SerializeField]
    private SoundParameters _soundData;

    private int level = 0;
    private AudioSource audioSource;

    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
            
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (!TryGetComponent<AudioSource>(out audioSource))
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = _musicLevels[level].audioClips[0];
        UpdateSettings();
        audioSource.Play();
    }

    public void UpdateSettings()
    {
        audioSource.volume = _soundData.volume;
        audioSource.pitch = _soundData.pitch;
        audioSource.spatialBlend = _soundData.spatialBlend;
        audioSource.dopplerLevel = _soundData.dopplerLevel;
        audioSource.rolloffMode = _soundData.rolloffMode;
        audioSource.minDistance = _soundData.minDistance;
        audioSource.maxDistance = _soundData.maxDistance;
        audioSource.playOnAwake = _soundData.playOnAwake;
        audioSource.loop = _soundData.looping;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextLevelMusic()
    {
        level++;
        StartCoroutine(FadeIt(_musicLevels[level].audioClips[0], _soundData.volume));
    }

    public void PreviousLevelMusic()
    {
        level--;
        StartCoroutine(FadeIt(_musicLevels[level].audioClips[0], _soundData.volume));
    }

    public void ChangeLevelMusic(int levelMusicIndex)
    {
        if (levelMusicIndex > 0 && levelMusicIndex < _musicLevels.Count)
        {
            level = levelMusicIndex;
            StartCoroutine(FadeIt(_musicLevels[level].audioClips[0], _soundData.volume));
        }
        else
        {
            Debug.LogError("Music index is out of bounds, please set index that is less or equal to music song count");
        }
    }

    public void ChangeSoundVolume(float level)
    {
        audioSource.volume = level;
    }

    public void ResetVolume()
    {
        audioSource.volume = _soundData.volume;
    }

    public void ChangePlaybackSpeed(float playbackSpeed)
    {
        audioSource.pitch = playbackSpeed;
        audioSource.outputAudioMixerGroup.audioMixer.SetFloat("MusicPich", 1f/ playbackSpeed);
    }
    
    IEnumerator FadeIt(AudioClip clip, float volume)
    {

        AudioSource originalAudioSource = GetComponent<AudioSource>();
        AudioSource fadeOutSource = gameObject.AddComponent<AudioSource>();
        fadeOutSource.clip = originalAudioSource.clip;
        fadeOutSource.time = originalAudioSource.time;
        fadeOutSource.volume = originalAudioSource.volume;
        fadeOutSource.pitch = originalAudioSource.pitch;
        fadeOutSource.loop = originalAudioSource.loop;
        fadeOutSource.outputAudioMixerGroup = originalAudioSource.outputAudioMixerGroup;
        
        fadeOutSource.Play();
        
        originalAudioSource.volume = 0f;
        originalAudioSource.clip = clip;
        float t = 0;
        float v = fadeOutSource.volume;
        originalAudioSource.Play();

        while (t < 0.98f)
        {
            t = Mathf.Lerp(t, 1f, Time.deltaTime * 0.8f);
            fadeOutSource.volume = Mathf.Lerp(v, 0f, t);
            originalAudioSource.volume = Mathf.Lerp(0f, volume, t);
            yield return null;
        }
        originalAudioSource.volume = volume;
        Destroy(fadeOutSource);
    }
}
