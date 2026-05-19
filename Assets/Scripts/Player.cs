using System;
using UnityEngine;

public class Player : StateMachineCore
{
    [Header("States")]
    [SerializeField] private PlayerMoveState _walk;
    [SerializeField] private PlayerMoveState _run;
    [SerializeField] private PlayerMoveState _crounch;
    [SerializeField] private PlayerDeadState _dead;
    [SerializeField] private LevelTransitionState _levelTransition;
    [SerializeField] private PlayerThrowState _throw;
    [SerializeField] private PlayerBookHandler _bookHandler;

    [Header("Components")]
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public bool IsHidden { get; private set; }
    public bool IsCrouching { get; private set; }
    public event Action<bool> OnCrouch;
    public PlayerBookHandler BookHandler => _bookHandler;
    public InputManager InputManager => _inputManager;
    private bool _isDead;
    public bool IsDead => _isDead;
    private bool _canThrowBook;

    private void Start()
    {
        SetupInstances();
        machine.Set(_walk);
    }

    private void Update()
    {
        if (_isDead)
        {
            machine.state.DoBranch();
            return;
        }

        SetFacingDirection();
        CheckThrow();
        SelectStates();
        machine.state.DoBranch();
    }
private void CheckThrow()
{
    if (_isDead) return;
    if (_bookHandler == null) return;
    if (!_bookHandler.HasBook)
    {
        _canThrowBook = false;
        return;
    }

    if (!_inputManager.InteractPressed)
    {
        _canThrowBook = true;
        return;
    }

    if (!_canThrowBook) return;
    if (IsTransitioning()) return;
    if (IsThrowing()) return;

    _canThrowBook = false;
    machine.Set(_throw);
}
    private bool IsThrowing()
    {
        return machine.state == _throw && !machine.state.isComplete;
    }
    private void FixedUpdate()
    {
        machine.state.FixedDoBranch();
    }

    private void SelectStates()
    {
        if (_isDead || IsTransitioning() || IsThrowing()) return;
        if (_inputManager.MoveInput.y < 0)
        {
            SetCrouch(true);
            machine.Set(_crounch);
        }
        else
        {
            SetCrouch(false);
            if (GameManager.Instance.PlayerSpotted)
            {
                machine.Set(_run);
            }
            else
            {
                machine.Set(_walk);
            }
        }
    }
    private void SetCrouch(bool crouch)
    {
        if (IsCrouching == crouch) return;

        IsCrouching = crouch;
        OnCrouch?.Invoke(IsCrouching);
        _spriteRenderer.sortingOrder = crouch ? -5 : 0;
    }

    private bool IsTransitioning()
    {
        return (machine.state == _levelTransition && !machine.state.isComplete);
    }

    public void ToggleHiding(bool hiding)
    {
        IsHidden = hiding;
    }

    public void SetPlayerDead()
    {
        if (_isDead) return;

        PlayerPrefs.SetInt("HasDiedBefore", 1);
        PlayerPrefs.Save();

        
        _isDead = true;
        _canThrowBook = false;
        machine.Set(_dead, true);
    }

    public void SetPlayerTransition(int direction)
    {
        _levelTransition.SetDirection(direction);
        machine.Set(_levelTransition);
    }
}
