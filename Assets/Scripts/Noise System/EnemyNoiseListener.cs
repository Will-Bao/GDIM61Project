using UnityEngine;

public class EnemyNoiseListener : MonoBehaviour
{
    [Header("Hearing Stats")]
    [SerializeField] private float _hearingRange = 5f;
    [SerializeField] private float _sensitivity = 1f;
    [SerializeField, Range(0,1)] private float _layerFalloff = 0.5f;

    [Header("Components")]
    [SerializeField] private LevelTracker _levelTracker;

    private Enemy _enemy;
    private void Start()
    {
        _enemy = GetComponent<Enemy>();
    }
    private void OnEnable()
    {
        NoiseManager.OnNoiseCreated += HandleNoise;
    }

    private void OnDisable()
    {
        NoiseManager.OnNoiseCreated -= HandleNoise;
    }

    private void HandleNoise(NoiseData data)
    {
        int layerDiff = Mathf.Abs(_levelTracker.CurrentLayer - data.Layer);
        float layerMultiplier = Mathf.Pow(_layerFalloff, layerDiff);

        float distance = Vector2.Distance(transform.position, data.Location);
        float effectiveRange = _hearingRange * data.Level * _sensitivity * layerMultiplier;

        if (distance <= effectiveRange)
        {
            _enemy.AlertEnemy(data);
        }
    }

    public void SetSensitivity(float newSensitivity)
    {
        _sensitivity = newSensitivity;
    }
}
