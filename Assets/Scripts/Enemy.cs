using System;
using UnityEngine;

public class Enemy : StateMachineCore
{
    [Header("States")]
    [SerializeField] private EnemyPatrolState _patrol;
    [SerializeField] private EnemyChaseState _chase;

    [Header("Player Detection")]
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private float _detectionRange;

    private Vector2 _targetPos;

    private void Start()
    {
        SetupInstances();
        machine.Set(_patrol);
    }

    private void Update()
    {
        SetFacingDirection();
        SelectStates();
        machine.state.DoBranch();
    }

    private void FixedUpdate()
    {
        machine.state.FixedDoBranch();
    }

    private void SelectStates()
    {
        if (CheckForPlayer())
        {
            _chase.SetTarget(_targetPos);
            machine.Set(_chase);
        }
        else
        {
            machine.Set(_patrol);
        }
    }

    private bool CheckForPlayer()
    {
        Collider2D[] detectedColliders = Physics2D.OverlapCircleAll(transform.position, _detectionRange, _playerLayer);
        foreach (var detected in detectedColliders)
        {
            if (detected.CompareTag("Player"))
            {
                Player player = detected.GetComponentInParent<Player>();
                if (!player.IsHidden)
                {
                    _targetPos = player.transform.position;
                    return true;
                }
            }
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponentInParent<Player>();
            if (!player.IsHidden)
            {
                GameManager.Instance.PlayerLose();
                player.SetPlayerDead();
            }
        }
    }
}
