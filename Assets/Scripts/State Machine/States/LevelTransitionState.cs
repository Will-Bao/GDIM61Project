using UnityEngine;

public class LevelTransitionState : State
{
    [Header("Animations")]
    [SerializeField] private AnimationClip _forward;
    [SerializeField] private AnimationClip _backward;
    [Header("Components")]
    [SerializeField] private MovementHandler _movementHandler;
    [Header("Settings")]
    [SerializeField] private float _transitionDuration;

    private float _initialAnimSpeed;
    private AnimationClip _selectedClip;
    private int _direction = 0; // -1 for backwards, 1 for forward

    public override void Enter()
    {
        if (_direction == 0) return;
        _selectedClip = _direction > 0 ? _forward : _backward;
        anim.Play(_selectedClip.name);
        _movementHandler.SetCanMove(false);
    }

    public override void Do()
    {
        if (time > _transitionDuration)
        {
            isComplete = true;
        }
    }

    public override void Exit()
    {
        _direction = 0;
        _movementHandler.SetCanMove(true);
    }

    public void SetDirection(int dir)
    {
        _direction = dir;
    }
}
