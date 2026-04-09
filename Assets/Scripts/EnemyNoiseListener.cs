using UnityEngine;

public class EnemyNoiseListener : MonoBehaviour
{
    [Header("Hearing Stats")]
    [SerializeField] private float _hearingRange = 5f;
    [SerializeField] private float _sensitivity = 1f;

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

    private void HandleNoise(Vector3 location, int noiseLevel)
    {
        float distance = Vector2.Distance(transform.position, location);
        float effectiveRange = _hearingRange * (noiseLevel * _sensitivity);

        if (distance <= effectiveRange)
        {
            _enemy.AlertEnemy(location);
        }
    }

    private void SetSensitivity(float newSensitivity)
    {
        _sensitivity = newSensitivity;
    }
}
