using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider _mainSlider;
    [SerializeField] private Slider _soundEffectsSlider;
    [SerializeField] private Slider _ambienceSlider;
    [Header("Mixer")]
    [SerializeField] private AudioMixer _mixer;
    [Header("Components")]
    [SerializeField] private GameObject _panel;
    [SerializeField] private Image _bgImg;
    [SerializeField] private MenuPanelFade _fade;

    private const string MAIN_VOLUME_KEY = "MainVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";
    private const string AMBIENCE_VOLUME_KEY = "AmbienceVolume";

    private bool _isOpened;

    private void Start()
    {
        ToggleSettingsMenu(false);
        LoadPlayerPrefs();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSettingsMenu(!_isOpened);
        }
    }

    public void ToggleSettingsMenu(bool isOpen)
    {
        if (isOpen) _panel.SetActive(true);
        else if (_fade != null) _fade.FadeOut();
        else _panel.SetActive(false);
        _bgImg.enabled = isOpen;
        _isOpened = isOpen;
    }

    public void SetMainVolume(float amount)
    {
        _mixer.SetFloat("MainVolume", amount);

        PlayerPrefs.SetFloat(MAIN_VOLUME_KEY, amount);
    }

    public void SetSoundEffectsVolume(float amount)
    {
        _mixer.SetFloat("SoundEffects", amount);

        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, amount);
    }

    public void SetAmbienceVolume(float amount)
    {
        _mixer.SetFloat("Ambience", amount);

        PlayerPrefs.SetFloat(AMBIENCE_VOLUME_KEY, amount);
    }

    private void LoadPlayerPrefs()
    {
        // Load saved values (default = 0 if nothing saved yet)
        float mainVolume = PlayerPrefs.GetFloat(MAIN_VOLUME_KEY, 0f);
        float sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 0f);
        float ambienceVolume = PlayerPrefs.GetFloat(AMBIENCE_VOLUME_KEY, 0f);

        // Set sliders
        _mainSlider.value = mainVolume;
        _soundEffectsSlider.value = sfxVolume;
        _ambienceSlider.value = ambienceVolume;

        // Apply to mixer
        SetMainVolume(mainVolume);
        SetSoundEffectsVolume(sfxVolume);
        SetAmbienceVolume(ambienceVolume);
    }
}