using UnityEngine;

public class ThrownBookEncounter : MonoBehaviour
{
    [SerializeField] private int _noiseLevel = 5;
    [SerializeField] private NoiseType _noiseType = NoiseType.Player;
    [SerializeField] private float _destroyDelay = 0.1f;

    private bool _hasMadeNoise;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_hasMadeNoise) return;
        _hasMadeNoise = true;
        NoiseManager.Instance.CreateNoise(
            new NoiseData(transform.position, _noiseLevel, LevelManager.Instance.CurrentLevel, _noiseType
            )
        );
        Destroy(gameObject, _destroyDelay);
    }
}