using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : State
{
    [Header("States")]
    [SerializeField] private AnimationState _idle;
    [SerializeField] private AnimationState _walk;

    [Header("Components")]
    [SerializeField] private MovementHandler _movement;

    [Header("Patrol")]
    [SerializeField] private List<Transform> _patrolPoints;
    [SerializeField] private float _patrolSpeed;
    [SerializeField] private float _stoppingDistance;

    [Header("Idle")]
    [SerializeField] private float _idleTime = 2f;

    private int _patrolIndex;
    private float _idleTimer;

    public override void Enter()
    {
        if (_patrolPoints == null)
        {
            Debug.Log("Enemy missing patrol points");
            return;
        }
        _idleTimer = _idleTime;
    }

    public override void Do()
    {
        if (_idleTimer > 0)
        {
            // Enemy idling
            machine.Set(_idle, true);
            _idleTimer -= Time.deltaTime;
            return;
        }

        Vector3 targetPos = _patrolPoints[_patrolIndex].position;
        if (Mathf.Abs(core.transform.position.x - targetPos.x) > _stoppingDistance)
        {
            // Enemy walking to patrol point
            Vector2 dir = targetPos - core.transform.position;
            _movement.SetHorizontalMovement(dir, _patrolSpeed);
            machine.Set(_walk, true);
        }
        else
        {
            // Select next patrol point
            _idleTimer = _idleTime;
            _patrolIndex = (_patrolIndex + 1) % _patrolPoints.Count;
            _movement.SetHorizontalMovement(Vector2.zero, _patrolSpeed);
        }
    }
}
