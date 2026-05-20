using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueSystems : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text _dialogue;
    [SerializeField] private GameObject _dialogueBox;

    [Header("Dialogue")]
    [SerializeField] private float _typeSpeed = 0.03f;
    [SerializeField] private bool _autoProgress = false;
    [SerializeField] private float _autoProgressDelay = 2f;


    private DialogueData _currentDialogue;
    private int _currentLine;
    private bool _isTalking;
    public bool IsTalking => _isTalking;
    private Coroutine _typingCoroutine;
    private bool _isTyping;

    private Coroutine _autoProgressCoroutine;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
                ContinueDialogue();
        }
    }

    public void StartDialogue(DialogueData dialogueData)
    {
        if (dialogueData == null) return;

        _currentDialogue = dialogueData;
        _currentLine = 0;
        _isTalking = true;

        _dialogueBox.SetActive(true);
        ShowLine();
    }

    public void ContinueDialogue()
    {
        if (!_isTalking) return;

        if (_autoProgressCoroutine != null)
        {
            StopCoroutine(_autoProgressCoroutine);
            _autoProgressCoroutine = null;
        }

        if (_isTyping)
        {
            StopCoroutine(_typingCoroutine);
            _dialogue.text = _currentDialogue.lines[_currentLine].text;
            _isTyping = false;
            return;
        }

        _currentLine++;

        if (_currentLine >= _currentDialogue.lines.Length)
        {
            EndDialogue();
            return;
        }

        ShowLine();
    }

    private void ShowLine()
    {
        if (_typingCoroutine != null)
        {
            StopCoroutine(_typingCoroutine);
        }

        _typingCoroutine = StartCoroutine(TypeLine(_currentDialogue.lines[_currentLine].text));
    }

    private void EndDialogue()
    {
        _isTalking = false;
        _dialogueBox.SetActive(false);
    }
    private IEnumerator TypeLine(string line)
    {
        _isTyping = true;
        _dialogue.text = "";

        foreach (char letter in line)
        {
            _dialogue.text += letter;
            yield return new WaitForSeconds(_typeSpeed);
        }

        _isTyping = false;

        if (_autoProgress)
        {
            if (_autoProgressCoroutine != null)
            {
                StopCoroutine(_autoProgressCoroutine);
            }

            _autoProgressCoroutine = StartCoroutine(AutoProgressLine());
        }
    }
    private IEnumerator AutoProgressLine()
    {
        yield return new WaitForSeconds(_autoProgressDelay);

        ContinueDialogue();
    }
    public void StartSingleLine(string line)
    {
        _currentDialogue = null;
        _currentLine = 0;
        _isTalking = true;

        _dialogueBox.SetActive(true);
        StartCoroutine(TypeLine(line));
    }
}