using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider _slider;
    [SerializeField] private SoundSettings _soundSettings;

    [Header("Channel")]
    [SerializeField] private VolumeChannelType _channel;

    private void OnEnable()
    {
        if (_slider == null)
        {
            Debug.LogError($"Slider is not assigned on {gameObject.name}", gameObject);
            return;
        }

        if (_soundSettings == null)
        {
            Debug.LogError($"SoundSettings is not assigned on {gameObject.name}", gameObject);
            return;
        }

        _slider.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnDisable()
    {
        if (_slider == null)
        {
            return;
        }

        _slider.onValueChanged.RemoveListener(OnValueChanged);
    }

    private void OnValueChanged(float value) =>
        _soundSettings.SetVolume(_channel, value);
}
