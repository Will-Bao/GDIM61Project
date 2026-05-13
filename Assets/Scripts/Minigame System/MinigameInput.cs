using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MinigameInput : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private MovementHandler _movement;
    [SerializeField] private InputManager _playerInput;

    public float XInput {  get; private set; }

    private bool _inMinigame;
    private float _lastX;

    private void Start()
    {
        MinigameManager.Instance.OnGameStarted += HandleMinigame;
        HandleMinigame(MinigameManager.Instance.GameStarted);
    }

    private void Update()
    {
        UpdateMinigameInput();
    }

    private void OnDestroy()
    {
        MinigameManager.Instance.OnGameStarted -= HandleMinigame;
    }
    private void HandleMinigame(bool isStarted)
    {
        _movement.SetCanMove(!isStarted);
        _inMinigame = isStarted;
    }

    private void UpdateMinigameInput()
    {
        if (!_inMinigame)
        {
            XInput = 0;
            return;
        }
        XInput = _playerInput.MoveInput.x;
    }
}
