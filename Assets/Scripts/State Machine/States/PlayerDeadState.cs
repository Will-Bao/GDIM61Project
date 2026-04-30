using UnityEngine;

public class PlayerDeadState : State
{
    [Header("Animation")]
    [SerializeField] private AnimationClip _deadAnim;

    [Header("Components")]
    [SerializeField] private MovementHandler _movementHandler;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Header("Settings")]
    [SerializeField] private float _invisDelay = 1f;
    public override void Enter()
    {
        anim.Play(_deadAnim.name);
        _movementHandler.SetCanMove(false);
    }

    public override void Do()
    {
        if (time > _deadAnim.length + _invisDelay)
        {
            _spriteRenderer.enabled = false;
        }
    }
}
