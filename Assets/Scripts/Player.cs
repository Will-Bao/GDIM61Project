using UnityEngine;

public class Player : StateMachineCore
{
    [Header("States")]
    [SerializeField] private PlayerMoveState _walk;
    [SerializeField] private PlayerMoveState _crounch;

    [Header("Components")]
    [SerializeField] private InputManager _inputManager;

    private void Start()
    {
        SetupInstances();
        machine.Set(_walk);
    }

    private void Update()
    {
        SetFacingDirection();
        SelectStates();
        machine.state.DoBranch();
    }

    private void FixedUpdate()
    {
        machine.state.FixedDoBranch();
    }

    private void SelectStates()
    {
        if (_inputManager.MoveInput.y < 0)
        {
            machine.Set(_crounch);
        }
        else
        {
            machine.Set(_walk);
        }
    }
}
