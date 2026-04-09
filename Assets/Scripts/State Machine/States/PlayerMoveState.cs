using UnityEngine;

public class PlayerMoveState : State
{
    [Header("States")]
    [SerializeField] private AnimationState _idle;
    [SerializeField] private AnimationState _walk;

    [Header("Components")]
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private MovementHandler _movement;

    [Header("Move Stats")]
    [SerializeField] private float _moveSpeed;

    [Header("Noise Settings")]
    [SerializeField] private int _noiseLevel = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Do()
    {
        SelectMoveState();
        HandleMovement();
    }

    private void SelectMoveState()
    {
        if (_inputManager.MoveInput.x == 0)
        {
            machine.Set(_idle, true);
        }
        else
        {
            machine.Set(_walk, true);
            NoiseManager.Instance.CreateNoise(core.transform.position, _noiseLevel);
        }
    }

    private void HandleMovement()
    {
        _movement.SetHorizontalMovement(_inputManager.MoveInput, _moveSpeed);
    }
}
