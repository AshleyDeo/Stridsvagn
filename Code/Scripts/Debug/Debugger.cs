using UnityEngine;
using TMPro;

public class Debugger : MonoBehaviour {
    public static Debugger Instance { get; private set; }
    public enum DisplayMode { FPS, MS }
    Canvas _canvas;
    [SerializeField] TextMeshProUGUI _display;
    [SerializeField] DisplayMode _displayMode = DisplayMode.FPS;
    [SerializeField, Range(0.1f, 2f)] float _sampleDuration = 1f;
    int _frames;
    float _duration, _bestDuration = float.MaxValue, worstDuratoin;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        _canvas = GetComponent<Canvas>();
    }
    private void Update() {
        if (!_canvas.enabled) return;
        float frameDuration = Time.unscaledDeltaTime;
        _frames += 1;
        _duration += frameDuration;
        if (frameDuration < _bestDuration) { _bestDuration = frameDuration; }
        if (frameDuration > worstDuratoin) { worstDuratoin = frameDuration; }
        if (_duration >= _sampleDuration) {
            if (_displayMode == DisplayMode.FPS) {
                _display.SetText(
                    "FPS\n{0:0}\n{1:0}\n{2:0}",
                    1f / _bestDuration,
                    _frames / _duration,
                    1f / worstDuratoin
                    );
            }
            else {
                _display.SetText(
                    "MS\n{0:1}\n{1:1}\n{2:1}",
                    1000f * _bestDuration,
                    1000f * _duration / _frames,
                    1000f * worstDuratoin
                    );
            }
            _frames = 0;
            _duration = 0f;
            _bestDuration = float.MaxValue;
            worstDuratoin = 0f;
        }
    }
    public void ToggleCanvas(bool isOn) => Debugger.Instance._canvas.enabled = isOn;
}