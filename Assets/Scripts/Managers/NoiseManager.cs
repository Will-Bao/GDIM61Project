using System;
using UnityEngine;

public class NoiseManager : MonoBehaviour
{
    public static NoiseManager Instance;
    public static Action<Vector3, int> OnNoiseCreated;
    [Header("Noise Settings")]
    [SerializeField] private int _maxNoise = 5;

    private int _currentNoiseLevel = 0;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(this);
    }

    public void CreateNoise(Vector3 location, int noiseLevel)
    {
        OnNoiseCreated?.Invoke(location, noiseLevel);
    }

    public void UpdateNoiseLevel(int newNoiseLevel)
    {
        if (newNoiseLevel > _maxNoise)
        {
            newNoiseLevel = _maxNoise;
        }
        else if (newNoiseLevel > _currentNoiseLevel)
        {
            _currentNoiseLevel = newNoiseLevel;
        }
    }
}
