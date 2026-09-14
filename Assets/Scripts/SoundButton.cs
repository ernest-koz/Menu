using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class SoundButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button _button;
    [SerializeField] private AudioSource _sound;
    [SerializeField] private SoundSettings _soundSettings;

    private Image _image;
    private Sprite _pressedSprite;
    private Sprite _normalSprite;
    private bool _isPressedView;

    private void Awake()
    {
        if (_button == null)
        {
            Debug.LogError($"Button is not assigned on {gameObject.name}", gameObject);
            return;
        }

        if (_sound == null)
        {
            Debug.LogError($"Sound is not assigned on {gameObject.name}", gameObject);
            return;
        }

        if (_soundSettings == null)
        {
            Debug.LogError($"SoundSettings is not assigned on {gameObject.name}", gameObject);
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

        if (_sound == null)
        {
            return;
        }

        _button.onClick.AddListener(OnButtonClicked);
        UpdateView();
    }

    private void Update()
    {
        if (_sound == null)
        {
            return;
        }

        if (_isPressedView && _sound.isPlaying == false)
        {
            UpdateView();
        }
    }

    private void OnDisable()
    {
        if (_button == null)
        {
            return;
        }

        _button.onClick.RemoveListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        if (_soundSettings.IsMuted)
        {
            return;
        }

        if (_sound.isPlaying)
        {
            _sound.Stop();
        }
        else
        {
            _sound.Play();
        }

        UpdateView();
    }

    private void UpdateView()
    {
        if (_pressedSprite == null)
        {
            return;
        }

        _isPressedView = _sound.isPlaying;

        if (_isPressedView)
        {
            _image.sprite = _pressedSprite;
        }
        else
        {
            _image.sprite = _normalSprite;
        }
    }
}
