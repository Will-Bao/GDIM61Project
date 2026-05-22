using UnityEngine;

public class ElevatorEncounter : MonoBehaviour
{
    [SerializeField] private ElevatorController _encounterController;
    [SerializeField] private LevelMarker _marker;
    [SerializeField] private bool _playOnlyOnce = true;

    private bool _hasPlayed;
    private void Awake()
    {
        if (_marker == null)
        {
            _marker = GetComponent<LevelMarker>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_playOnlyOnce && _hasPlayed) return;
        if (!other.GetComponentInParent<InputManager>()) return;

        if (LevelManager.Instance == null)
        {
            Debug.LogWarning("No LevelManager in scene.");
            return;
        }

        if (_marker != null && LevelManager.Instance.CurrentLevel != _marker.CurrentLayer) return;

        if (_encounterController == null)
        {
            Debug.LogError("ElevatorController is not assigned.");
            return;
        }

        _hasPlayed = true;
        _encounterController.StartEncounter();
    }
}