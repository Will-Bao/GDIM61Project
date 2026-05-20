using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueSystems _dialogueSystem;
    [SerializeField] private DialogueData _dialogueData;
    [SerializeField] private bool _playOnlyOnce = true;

    private bool _hasPlayed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_playOnlyOnce && _hasPlayed) return;

        if (other.CompareTag("Player"))
        {
            _dialogueSystem.StartDialogue(_dialogueData);
            _hasPlayed = true;
        }
    }
}