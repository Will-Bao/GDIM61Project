using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MinigameInput : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private MovementHandler _movement;
    [SerializeField] private InputManager _playerInput;

    private bool _inMinigame;
    private float _lastX;

    private void Start()
    {
        BeatGameManager.Instance.BeatGameStarted += HandleMinigame;
        HandleMinigame(BeatGameManager.Instance.GameStarted);
    }

    private void Update()
    {
        UpdateMinigameInput();
    }

    private void OnDestroy()
    {
        BeatGameManager.Instance.BeatGameStarted -= HandleMinigame;
    }
    private void HandleMinigame(bool isStarted)
    {
        _movement.SetCanMove(!isStarted);
        _inMinigame = isStarted;
    }

    private void UpdateMinigameInput()
    {
        if (!_inMinigame) return;
        float currentX = _playerInput.MoveInput.x;

        if (_lastX == 0 && currentX < 0)
            BeatGameManager.Instance.TryHit(-1);
        else if (_lastX == 0 && currentX > 0)
            BeatGameManager.Instance.TryHit(1);

        _lastX = currentX;
    }
}
