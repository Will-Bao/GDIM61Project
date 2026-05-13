using System.Collections;
using UnityEngine;

public class IntroSequenceController : MonoBehaviour
{
    [Header("Intro Object")]
    [SerializeField] private GameObject _introObject;
    [SerializeField] private GameObject _playerUI;

    [SerializeField] private Animator _introAnimator;
    [SerializeField] private string _introAnimationName;
    [SerializeField] private float _introAnimationLength = 10f;

    [Header("Player")]
    [SerializeField] private GameObject _player;

    [Header("Dialogue")]
    [SerializeField] private DialogueSystems _dialogueSystem;
    [SerializeField] private DialogueData _introDialogue;

    private IEnumerator Start()
    {
        _player.SetActive(false);
        _introObject.SetActive(true);
        _playerUI.SetActive(false);

        _introAnimator.Play(_introAnimationName);

        yield return new WaitForSeconds(_introAnimationLength);

        _dialogueSystem.StartDialogue(_introDialogue);

        yield return new WaitUntil(() => !_dialogueSystem.IsTalking);

        _introObject.SetActive(false);
        _player.SetActive(true);
        _playerUI.SetActive(true);
    }
}