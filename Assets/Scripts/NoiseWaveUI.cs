using UnityEngine;
public class NoiseWaveUI : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private Animator _animator;

    private void OnEnable()
    {
        NoiseManager.OnNoiseCreated += PlayNoiseAnimation;
    }

    private void OnDisable()
    {
        NoiseManager.OnNoiseCreated -= PlayNoiseAnimation;
    }

    private void PlayNoiseAnimation(NoiseData noise)
    {
        float noisePercent = (float)noise.Level / NoiseManager.Instance.MaxNoise;

        if (noisePercent <= 0.25f)
        {
            _animator.SetTrigger("LowNoise");
        }
        else if (noisePercent <= 0.6f)
        {
            _animator.SetTrigger("MediumNoise");
        }
    }
}