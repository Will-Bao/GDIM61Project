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
    [SerializeField] private GameObject _dialogueBox;

    [SerializeField] private DialogueData _introDialogue;

    [Header("Skip UI")]
    [SerializeField] private GameObject _skipPrompt;
    [Header("ControlScheme")]
    [SerializeField] private GameObject _controls;


    private bool _skipRequested;
    private bool _canSkip;

    private IEnumerator Start()
    {
        _canSkip = PlayerPrefs.GetInt("HasDiedBefore", 0) == 1;
        _controls.SetActive(false);
        _player.SetActive(false);
        _introObject.SetActive(true);
        _playerUI.SetActive(false);
        _skipPrompt.SetActive(_canSkip);

        _introAnimator.Play(_introAnimationName);

        float timer = 0f;

        while (timer < _introAnimationLength && !_skipRequested)
        {
            if (_canSkip && Input.GetKeyDown(KeyCode.Escape))
            {
                _skipRequested = true;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        if (!_skipRequested)
        {
            _dialogueSystem.StartDialogue(_introDialogue);

            yield return new WaitUntil(() =>
            {
                if (_canSkip && Input.GetKeyDown(KeyCode.Escape))
                {
                    _skipRequested = true;
                }

                return !_dialogueSystem.IsTalking || _skipRequested;
            });
        }
        if (_skipRequested)
        {
            _dialogueBox.SetActive(false);
        }
        StartCoroutine(EndIntro());
    }

    private IEnumerator EndIntro()
    {
        _skipPrompt.SetActive(false);
        _introObject.SetActive(false);
        _player.SetActive(true);
        _playerUI.SetActive(true);
        _dialogueBox.SetActive(false);

        _controls.SetActive(true);

        yield return new WaitForSeconds(3f);

        _controls.SetActive(false);
    }
}