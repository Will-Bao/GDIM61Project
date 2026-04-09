using System;
using UnityEngine;

/// <summary>
/// Manages and broadcast noise within the game.
/// </summary>
public class NoiseManager : MonoBehaviour
{
    public static NoiseManager Instance;
    public static Action<Vector3, int> OnNoiseCreated;
    [Header("Noise Settings")]
    [SerializeField] private int _maxNoise = 5;

    public int MaxNoise => _maxNoise;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(this);
    }

    /// <summary>
    /// Creates noise at the specified location and amount
    /// </summary>
    /// <param name="location"> Where the noise originates from. </param>
    /// <param name="noiseLevel"> Determines far away the noise can be heard. </param>
    public void CreateNoise(Vector3 location, int noiseLevel)
    {
        if (noiseLevel < 0) return;
        if (noiseLevel > MaxNoise) noiseLevel = MaxNoise;
        OnNoiseCreated?.Invoke(location, noiseLevel);
    }
}
