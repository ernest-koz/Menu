using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    
    [SerializeField] private AudioSource[] buttonSounds;
    [SerializeField] private AudioSource backgroundMusic;
    
    [SerializeField] private Button toggleSoundButton;
    [SerializeField] private Button[] soundButtons;
    
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider buttonsVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    
    private const string MasterVolumeParam = "MasterVol";
    private const string ButtonsVolumeParam = "ButtonsVol";
    private const string MusicVolumeParam = "MusicVol";
    
    private bool isSoundEnabled = true;
    private bool[] isButtonSoundPlaying;
    private float lastMasterVolume = 1f;
    private float lastButtonsVolume = 1f;
    private float lastMusicVolume = 1f;

    private void Awake()
    {
        isButtonSoundPlaying = new bool[buttonSounds.Length];
    }

    private void Start()
    {
        InitializeSliders();
        SetupListeners();
        UpdateButtonText();
        
        SetVolume(MasterVolumeParam, lastMasterVolume);
        SetVolume(ButtonsVolumeParam, lastButtonsVolume);
        SetVolume(MusicVolumeParam, lastMusicVolume);
    }

    private void InitializeSliders()
    {
        masterVolumeSlider.minValue = 0.0001f;
        masterVolumeSlider.maxValue = 1f;
        masterVolumeSlider.value = 1f;

        buttonsVolumeSlider.minValue = 0.0001f;
        buttonsVolumeSlider.maxValue = 1f;
        buttonsVolumeSlider.value = 1f;

        musicVolumeSlider.minValue = 0.0001f;
        musicVolumeSlider.maxValue = 1f;
        musicVolumeSlider.value = 1f;
    }

    private void SetupListeners()
    {
        masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        buttonsVolumeSlider.onValueChanged.AddListener(OnButtonsVolumeChanged);
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        toggleSoundButton.onClick.AddListener(ToggleSound);
        
        for (int i = 0; i < soundButtons.Length; i++)
        {
            int index = i;
            soundButtons[i].onClick.AddListener(() => ToggleButtonSound(index));
        }
    }

    private void OnMasterVolumeChanged(float value)
    {
        lastMasterVolume = value;
        SetVolume(MasterVolumeParam, value);
    }

    private void OnButtonsVolumeChanged(float value)
    {
        lastButtonsVolume = value;
        SetVolume(ButtonsVolumeParam, value);
    }

    private void OnMusicVolumeChanged(float value)
    {
        lastMusicVolume = value;
        SetVolume(MusicVolumeParam, value);
    }

    private void SetVolume(string parameter, float value)
    {
        float decibels = Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20;
        audioMixer.SetFloat(parameter, decibels);
    }

    private void ToggleSound()
    {
        isSoundEnabled = !isSoundEnabled;

        if (isSoundEnabled)
        {
            SetVolume(MasterVolumeParam, lastMasterVolume);
            SetVolume(ButtonsVolumeParam, lastButtonsVolume);
            SetVolume(MusicVolumeParam, lastMusicVolume);
        }
        else
        {
            audioMixer.SetFloat(MasterVolumeParam, -80f);
            audioMixer.SetFloat(ButtonsVolumeParam, -80f);
            audioMixer.SetFloat(MusicVolumeParam, -80f);
        }

        UpdateButtonText();
    }

    private void UpdateButtonText()
    {
        TextMeshProUGUI buttonText = toggleSoundButton.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            buttonText.text = isSoundEnabled ? "ВЫКЛЮЧИТЬ ЗВУК" : "ВКЛЮЧИТЬ ЗВУК";
        }
    }

    private void ToggleButtonSound(int index)
    {
        if (!isSoundEnabled)
            return;
            
        if (index < 0 || index >= buttonSounds.Length)
            return;

        isButtonSoundPlaying[index] = !isButtonSoundPlaying[index];

        if (isButtonSoundPlaying[index])
        {
            buttonSounds[index].Play();
        }
        else
        {
            buttonSounds[index].Stop();
        }
    }

    private void OnDisable()
    {
        masterVolumeSlider.onValueChanged.RemoveListener(OnMasterVolumeChanged);
        buttonsVolumeSlider.onValueChanged.RemoveListener(OnButtonsVolumeChanged);
        musicVolumeSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
        toggleSoundButton.onClick.RemoveListener(ToggleSound);
        
        for (int i = 0; i < soundButtons.Length; i++)
        {
            soundButtons[i].onClick.RemoveListener(() => ToggleButtonSound(i));
        }
    }
}