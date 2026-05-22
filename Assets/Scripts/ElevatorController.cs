using System.Collections;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    [Header("Gameplay")]
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _playerUI;

    [Header("Encounter")]
    [SerializeField] private GameObject _encounterVisuals;
    [SerializeField] private Animator _encounterAnimator;
    [SerializeField] private string _animationName;
    [SerializeField] private float _animationLength = 2f;

    [Header("Dialogue")]
    [SerializeField] private DialogueSystems _dialogueSystem;
    [SerializeField] private DialogueData _encounterDialogue;

    private bool _isPlaying;

    public void StartEncounter()
    {
        if (_isPlaying) return;
        StartCoroutine(EncounterRoutine());
    }

    private IEnumerator EncounterRoutine()
    {
        _isPlaying = true;

        _player.SetActive(false);
        _playerUI.SetActive(false);

        _encounterVisuals.SetActive(true);

        if (_encounterAnimator != null)
        {
            _encounterAnimator.Play(_animationName);
            yield return new WaitForSeconds(_animationLength);
        }

        if (_encounterDialogue != null)
        {
            _dialogueSystem.StartDialogue(_encounterDialogue);
            yield return new WaitUntil(() => !_dialogueSystem.IsTalking);
        }

        _encounterVisuals.SetActive(false);

        _player.SetActive(true);
        _playerUI.SetActive(true);

        _isPlaying = false;
    }
}