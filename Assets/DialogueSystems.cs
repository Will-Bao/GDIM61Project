using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueSystems : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text _dialogue;
    [SerializeField] private GameObject _dialogueBox;

    [Header("Test")]
    [SerializeField] private DialogueData _testDialogue;
    [SerializeField] private float _typeSpeed = 0.03f;


    private DialogueData _currentDialogue;
    private int _currentLine;
    private bool _isTalking;
    public bool IsTalking => _isTalking;
    private Coroutine _typingCoroutine;
    private bool _isTyping;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!_isTalking)
            {
                StartDialogue(_testDialogue);
            }
            else
            {
                ContinueDialogue();
            }
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
    }
}