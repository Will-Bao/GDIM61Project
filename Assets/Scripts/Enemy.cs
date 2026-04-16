using System;
using Unity.Cinemachine;
using UnityEngine;

public class Enemy : StateMachineCore
{
    [Header("States")]
    [SerializeField] private EnemyPatrolState _patrol;
    [SerializeField] private EnemyChaseState _chase;
    [SerializeField] private EnemyAttackState _attack;

    [Header("Components")]
    [SerializeField] private CinemachineImpulseSource _cameraImpulse;

    [Header("Player Detection")]
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private float _detectionRange;

    [Header("Proximity Shake")]
    [SerializeField] private float _shakeAmount;
    [SerializeField] private float _shakeDelay;

    [Header("Proximity Audio")]
    [SerializeField] private AudioClip _proximityAudio;

    private Vector2 _targetPos;
    private float _shakeTimer = 0f;

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
        if (IsChasing() || IsAttacking()) return;
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

    public void AlertEnemy(Vector2 _alertedPos)
    {
        _chase.SetTarget(_alertedPos);
        machine.Set(_chase);
    }

    private bool CheckForPlayer()
    {
        Collider2D[] detectedColliders = Physics2D.OverlapCircleAll(transform.position, _detectionRange, _playerLayer);
        foreach (var detected in detectedColliders)
        {
            if (detected.CompareTag("Player"))
            {
                HandleProximityShake();
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

    private bool IsChasing()
    {
        return (machine.state == _chase) && (!machine.state.isComplete);
    }

    private bool IsAttacking()
    {
        return (machine.state == _attack) && (!machine.state.isComplete);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponentInParent<Player>();
            if (!player.IsHidden)
            {
                machine.Set(_attack);
                player.SetPlayerDead();
            }
        }
    }

    private void HandleProximityShake()
    {
        if (Mathf.Abs(rb.linearVelocityX) <= 0) return; // return if not moving
        _shakeTimer += Time.deltaTime;
        if (_shakeTimer > _shakeDelay)
        {
            _shakeTimer = 0f;
            _cameraImpulse.GenerateImpulse(_shakeAmount);
            SoundFXManager.instance.PlaySoundFXClip(_proximityAudio, transform, 2f, regulated: false, randPitch: true);
        }
    }
}
