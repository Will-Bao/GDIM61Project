using UnityEngine;

public class EnemyAttackState : State
{
    [Header("Animation")]
    [SerializeField] private AnimationClip _attackAnim;

    [Header("Component")]
    [SerializeField] private MovementHandler _movement;
    public override void Enter()
    {
        anim.Play(_attackAnim.name);
        _movement.SetCanMove(false);
    }

    public override void Do()
    {
        if (time > _attackAnim.length)
        {
            GameManager.Instance.PlayerLose();
        }
    }
}
