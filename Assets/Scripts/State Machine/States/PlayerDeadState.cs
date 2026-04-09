using UnityEngine;

public class PlayerDeadState : State
{
    [Header("Animation")]
    [SerializeField] private AnimationClip _deadAnim;

    [Header("Components")]
    [SerializeField] private MovementHandler _movementHandler;
    public override void Enter()
    {
        anim.Play(_deadAnim.name);
        _movementHandler.SetCanMove(false);
    }
}
