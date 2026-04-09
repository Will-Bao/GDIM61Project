using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class Player : StateMachineCore
{
    [Header("States")]
    [SerializeField] private PlayerMoveState _walk;
    [SerializeField] private PlayerMoveState _crounch;
    [SerializeField] private PlayerDeadState _dead;

    [Header("Components")]
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public bool IsHidden { get; private set; }
    public bool IsCrouching { get; private set; }
    public event Action<bool> OnCrouch;

    private bool _isDead;

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
        if (_isDead) return;
        if (_inputManager.MoveInput.y < 0)
        {
            SetCrouch(true);
            machine.Set(_crounch);
        }
        else
        {
            SetCrouch(false);
            machine.Set(_walk);
        }
    }
    public void SetCrouch(bool crouch)
    {
        if (IsCrouching == crouch) return;

        IsCrouching = crouch;
        OnCrouch?.Invoke(IsCrouching);
        _spriteRenderer.sortingOrder = crouch ? -5 : 0;
    }

    public void ToggleHiding(bool hiding)
    {
        IsHidden = hiding;
    }

    public void SetPlayerDead()
    {
        machine.Set(_dead);
        _isDead = true;
    }
}
