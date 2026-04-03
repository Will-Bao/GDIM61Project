using UnityEngine;

/// <summary>
/// A state for handling animation.
/// </summary>
public class AnimationState : State
{
    [Header("Animation")]
    [SerializeField] private AnimationClip _animClip;

    public override void Enter()
    {
        if (_animClip) anim.Play(_animClip.name);
    }
}
