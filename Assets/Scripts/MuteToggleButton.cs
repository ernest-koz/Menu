using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class MuteToggleButton : MonoBehaviour
{
    private const string TurnOffSoundText = "ВЫКЛЮЧИТЬ ЗВУК";
    private const string TurnOnSoundText = "ВКЛЮЧИТЬ ЗВУК";

    [Header("References")]
    [SerializeField] private Button _button;
    [SerializeField] private SoundSettings _soundSettings;
    [SerializeField] private TextMeshProUGUI _label;

    private Image _image;
    private Sprite _pressedSprite;
    private Sprite _normalSprite;

    private void Awake()
    {
        if (_button == null)
        {
            Debug.LogError($"Button is not assigned on {gameObject.name}", gameObject);
            return;
        }

        if (_soundSettings == null)
        {
            Debug.LogError($"SoundSettings is not assigned on {gameObject.name}", gameObject);
            return;
        }

        if (_label == null)
        {
            Debug.LogError($"Label is not assigned on {gameObject.name}", gameObject);
            return;
        }

        _image = GetComponent<Image>();

        if (_button.spriteState.pressedSprite == null)
        {
            Debug.LogError($"Pressed Sprite is not assigned in the Button component on {gameObject.name}", gameObject);
            return;
        }

        _pressedSprite = _button.spriteState.pressedSprite;
        _normalSprite = _image.sprite;
    }

    private void OnEnable()
    {
        if (_button == null)
        {
            return;
        }

        if (_soundSettings == null)
        {
            return;
        }

        if (_label == null)
        {
            return;
        }

        _button.onClick.AddListener(OnButtonClicked);
        _soundSettings.MuteChanged += OnMuteChanged;

        UpdateLabel(_soundSettings.IsMuted);
        UpdateView(_soundSettings.IsMuted);
    }

    private void OnDisable()
    {
        if (_soundSettings == null)
        {
            return;
        }

        _soundSettings.MuteChanged -= OnMuteChanged;

        if (_button == null)
        {
            return;
        }

        _button.onClick.RemoveListener(OnButtonClicked);
    }

    private void OnButtonClicked() =>
        _soundSettings.ToggleMute();

    private void OnMuteChanged(bool isMuted)
    {
        UpdateLabel(isMuted);
        UpdateView(isMuted);
    }

    private void UpdateLabel(bool isMuted)
    {
        _label.text = isMuted ? TurnOnSoundText : TurnOffSoundText;
    }

    private void UpdateView(bool isMuted)
    {
        if (_pressedSprite == null)
        {
            return;
        }

        if (isMuted)
        {
            _image.sprite = _pressedSprite;
        }
        else
        {
            _image.sprite = _normalSprite;
        }
    }
}
