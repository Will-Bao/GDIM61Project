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

    private void HandleNoise(NoiseData data)
    {
        float distance = Vector2.Distance(transform.position, data.Location);
        float effectiveRange = _hearingRange * (data.Level * _sensitivity);

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
