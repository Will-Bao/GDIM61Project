using UnityEngine;

public class EnemyAttackState : State
{
    [Header("Animation")]
    [SerializeField] private AnimationClip _attackAnim;
    [SerializeField] private AnimationClip _killAnim;

    [Header("Component")]
    [SerializeField] private MovementHandler _movement;

    private bool _killToggle;
    public override void Enter()
    {
        anim.Play(_attackAnim.name);
        _movement.SetCanMove(false);
        _killToggle = false;
    }

    public override void Do()
    {
        if (time > _attackAnim.length && !_killToggle)
        {
            _killToggle = true;
            anim.Play(_killAnim.name);
        }
        if (time > _killAnim.length + _attackAnim.length)
        {
            GameManager.Instance.PlayerLose();
        }
    }
}
