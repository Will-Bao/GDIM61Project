using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueSystems _dialogueSystem;
    [SerializeField] private DialogueData _dialogueData;
    [SerializeField] private LevelMarker _marker;
    [SerializeField] private bool _playOnlyOnce = true;

    private bool _hasPlayed;

    private void Awake()
    {
        if (_marker == null)
            _marker = GetComponent<LevelMarker>();

        if (_dialogueSystem == null)
            _dialogueSystem = FindFirstObjectByType<DialogueSystems>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_playOnlyOnce && _hasPlayed) return;
        if (!other.GetComponentInParent<InputManager>()) return;

        if (_marker != null && LevelManager.Instance.CurrentLevel != _marker.CurrentLayer) return;
        Debug.Log("Starting Dialogue");
        _dialogueSystem.StartDialogue(_dialogueData);
        _hasPlayed = true;
    }
}