using UnityEngine;

public class Player : StateMachineCore
{
    [Header("States")]
    [SerializeField] private AnimationState _idle;

    [Header("Components")]
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private MovementHandler _movement;

    [Header("Stats")]
    [SerializeField] private float _moveSpeed;

    private void Start()
    {
        SetupInstances();
        machine.Set(_idle);
    }

    private void Update()
    {
        SetFacingDirection();
        HandleMovement();
        SelectStates();
        machine.state.DoBranch();
    }

    private void FixedUpdate()
    {
        machine.state.FixedDoBranch();
    }

    private void SelectStates()
    {
        machine.Set(_idle);
    }

    private void HandleMovement()
    {
        _movement.SetHorizontalMovement(_inputManager.MoveInput, _moveSpeed);
    }
}
