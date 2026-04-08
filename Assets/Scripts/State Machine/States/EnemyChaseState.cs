using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyChaseState : State
{
    [Header("States")]
    [SerializeField] private AnimationState _chase;
    [SerializeField] private AnimationState _idle; // Change to attack later

    [Header("Components")]
    [SerializeField] private MovementHandler _movement;

    [Header("Chase")]
    [SerializeField] private float _chaseSpeed;
    [SerializeField] private float _stoppingDistance;

    private Vector2 _targetPos;

    public override void Enter()
    {
        if (_targetPos == null)
        {
            Debug.Log("Enemy missing target");
            return;
        }
    }

    public override void Do()
    {
        if (Mathf.Abs(core.transform.position.x - _targetPos.x) > _stoppingDistance)
        {
            // Enemy chasing to target
            Vector2 dir = _targetPos - (Vector2)core.transform.position;
            _movement.SetHorizontalMovement(dir, _chaseSpeed);
            machine.Set(_chase, true);
        }
        else
        {
            _movement.SetHorizontalMovement(Vector2.zero, 0);
            machine.Set(_idle, true);
            isComplete = true;
        }
    }

    public override void Exit()
    {
        _movement.SetHorizontalMovement(Vector2.zero, 0);
    }

    public void SetTarget(Vector2 target)
    {
        _targetPos = target;
    }
}
