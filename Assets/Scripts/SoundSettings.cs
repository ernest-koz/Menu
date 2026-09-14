using System;
using UnityEngine;
using UnityEngine.Audio;

public class SoundSettings : MonoBehaviour
{
    private const float MinimumLinearVolume = 0.0001f;
    private const float MaximumLinearVolume = 1f;
    private const float MutedDecibels = -80f;
    private const float DecibelsPerBel = 20f;

    private const string MasterVolumeParameter = "MasterVol";
    private const string ButtonsVolumeParameter = "ButtonsVol";
    private const string MusicVolumeParameter = "MusicVol";

    private static readonly string[] VolumeParameters =
    {
        MasterVolumeParameter,
        ButtonsVolumeParameter,
        MusicVolumeParameter,
    };

    [SerializeField] private AudioMixer _audioMixer;

    private float[] _lastVolumes;
    private bool _isMuted;

    public event Action<bool> MuteChanged;

    public bool IsMuted => _isMuted;

    private void Awake()
    {
        if (_audioMixer == null)
        {
            Debug.LogError($"AudioMixer is not assigned on {gameObject.name}", gameObject);
            return;
        }

        _lastVolumes = new float[VolumeParameters.Length];

        for (int i = 0; i < _lastVolumes.Length; i++)
        {
            _lastVolumes[i] = MaximumLinearVolume;
        }
    }

    public void SetVolume(VolumeChannelType channel, float linearValue)
    {
        if (_audioMixer == null)
        {
            return;
        }

        _lastVolumes[(int)channel] = linearValue;

        if (_isMuted == false)
        {
            ApplyVolume(channel, linearValue);
        }
    }

    public void ToggleMute()
    {
        if (_audioMixer == null)
        {
            return;
        }

        _isMuted = _isMuted == false;

        if (_isMuted)
        {
            MuteAll();
        }
        else
        {
            RestoreAll();
        }

        MuteChanged?.Invoke(_isMuted);
    }

    private void ApplyVolume(VolumeChannelType channel, float linearValue) =>
        _audioMixer.SetFloat(VolumeParameters[(int)channel], LinearToDecibels(linearValue));

    private void MuteAll()
    {
        for (int i = 0; i < VolumeParameters.Length; i++)
        {
            _audioMixer.SetFloat(VolumeParameters[i], MutedDecibels);
        }
    }

    private void RestoreAll()
    {
        for (int i = 0; i < VolumeParameters.Length; i++)
        {
            ApplyVolume((VolumeChannelType)i, _lastVolumes[i]);
        }
    }

    private static float LinearToDecibels(float linearValue) =>
        Mathf.Log10(Mathf.Max(linearValue, MinimumLinearVolume)) * DecibelsPerBel;
}
