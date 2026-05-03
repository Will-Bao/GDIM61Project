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
        int level = noise.Level;

        if (level == 0)
        {
            return;
        }
        else if (level == 1)
        {
            _animator.SetTrigger("LowNoise");
        }
        else if (level == 2)
        {
            _animator.SetTrigger("MediumNoise");
        }
        else if (level == 5)
        {
            _animator.SetTrigger("LoudNoise");
        }
    }
}