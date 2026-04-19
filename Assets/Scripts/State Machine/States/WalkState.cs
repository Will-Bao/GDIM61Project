using UnityEngine;

public class WalkState : State
{
    [Header("Animation")]
    [SerializeField] private AnimationClip _walkAnim;
    [SerializeField] private float _maxAnimSpeed = 5f;

    private float _initialSpeed;
    public override void Enter()
    {
        anim.Play(_walkAnim.name);
        _initialSpeed = anim.speed;
    }

    public override void Do()
    {
        float velocity = rb.linearVelocity.magnitude;

        anim.speed = velocity / _maxAnimSpeed;
    }

    public override void Exit()
    {
        anim.speed = _initialSpeed;
    }
}
