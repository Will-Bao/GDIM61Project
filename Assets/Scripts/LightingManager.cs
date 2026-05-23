using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightingManager : MonoBehaviour
{
    public static LightingManager Instance;

    [SerializeField] private Light2D _globalLight;

    [SerializeField] private float _normalIntensity = 1f;
    [SerializeField] private float _hiddenIntensity = 0.35f;
    [SerializeField] private float _fadeSpeed = 5f;

    private float _targetIntensity;

    private void Awake()
    {
        Instance = this;
        FindGlobalLight();
        _targetIntensity = _normalIntensity;
    }
    private void Update()
    {
        if (_globalLight == null)
        {
            FindGlobalLight();
            return;
        }

        _globalLight.intensity = Mathf.Lerp(
            _globalLight.intensity,
            _targetIntensity,
            Time.deltaTime * _fadeSpeed
        );
    }

    private void FindGlobalLight()
    {
        _globalLight = FindFirstObjectByType<Light2D>();
    }

    public void SetTableHidingDarkness(bool isHiding)
    {
        _targetIntensity = isHiding ? _hiddenIntensity : _normalIntensity;
    }
}