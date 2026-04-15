using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;
    [Header("Theme Music")]
    [SerializeField] private AudioClip _themeMusic;
    [SerializeField] private float _themeVolume = 1f;
    [Header("References")]
    [SerializeField] private AudioSource soundFXObject;
    [SerializeField] private AudioSource _themeAudioSource;
    private Dictionary<AudioClip, AudioSource> _currentSFX = new Dictionary<AudioClip, AudioSource>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        if (_themeMusic != null)
        {
            PlayThemeMusic(_themeMusic, _themeVolume);
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume, bool loop = false, bool regulated = true, bool randPitch = false)
    {
        if (_currentSFX.ContainsKey(audioClip)) return;
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
        if (regulated) _currentSFX.Add(audioClip, audioSource);
        audioSource.clip = audioClip;
        if (randPitch) audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.Play();
        float clipLength = audioSource.clip.length;
        if (!loop)
        {
            StartCoroutine(RemoveAudioClip(audioClip, clipLength));
            Destroy(audioSource.gameObject, clipLength);
        }
    }

    public void PlayRandomSoundFXClip(AudioClip[] audioClip, Transform spawnTransform, float volume, bool loop = false, bool randPitch = false)
    {
        int rand = Random.Range(0, audioClip.Length);
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
        if (randPitch) audioSource.pitch = Random.Range(-0.5f, 0.5f);
        audioSource.clip = audioClip[rand];
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.Play();
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }

    public void StopSFX(AudioClip audioClip)
    {
        if (_currentSFX.ContainsKey(audioClip))
        {
            Destroy(_currentSFX[audioClip].gameObject);
            _currentSFX.Remove(audioClip);
        }
        else
        {
            Debug.Log($"Sound effect: {audioClip.name} not found");
        }
    }

    private void PlayThemeMusic(AudioClip music, float volume)
    {
        _themeAudioSource.clip = music;
        _themeAudioSource.volume = volume;
        _themeAudioSource.loop = true;
        _themeAudioSource.Play();
    }

    private IEnumerator RemoveAudioClip(AudioClip clip, float delay)
    {
        yield return new WaitForSeconds(delay);
        _currentSFX.Remove(clip);
    }
}
