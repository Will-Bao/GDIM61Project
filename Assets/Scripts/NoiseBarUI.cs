using UnityEngine;
using UnityEngine.UI;

public class NoiseBarUI : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private Slider _noiseBar;

    [Header("Bar Settings")]
    [SerializeField] private float _barSpeed;

    private float _targetValue;

    private void Start()
    {
        _noiseBar.value = 0;
        _noiseBar.maxValue = 100;
    }

    private void OnEnable()
    {
        NoiseManager.OnNoiseCreated += UpdateNoiseLevel;
    }
    private void OnDisable()
    {
        NoiseManager.OnNoiseCreated -= UpdateNoiseLevel;
    }

    private void UpdateNoiseLevel(NoiseData noise)
    {
        _targetValue = ((float)noise.Level / NoiseManager.Instance.MaxNoise) * 100;
    }

    private void Update()
    {
        UpdateNoiseDisplay();
    }
    private void UpdateNoiseDisplay()
    {
        if (_noiseBar.value == 0 && _targetValue == 0) return;

        _noiseBar.value = Mathf.Lerp(_noiseBar.value, _targetValue, Time.deltaTime * _barSpeed);

        if (Mathf.Abs(_noiseBar.value - _targetValue) < 0.1 && _targetValue != 0)
        {
            _targetValue = 0;
        }
        else if (_noiseBar.value < 0.1f) _noiseBar.value = 0;
    }
}
