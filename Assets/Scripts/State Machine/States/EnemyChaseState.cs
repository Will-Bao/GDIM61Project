using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Cinemachine;

public class EnemyChaseState : State
{
    [Header("States")]
    [SerializeField] private AnimationState _chase;
    [SerializeField] private AnimationState _idle; // Change to attack later

    [Header("Components")]
    [SerializeField] private MovementHandler _movement;
    [SerializeField] private CinemachineImpulseSource _impulseSource;

    [Header("Audio")]
    [SerializeField] private AudioClip _footstepAudioClip;

    [Header("Chase")]
    [SerializeField] private float _chaseSpeed;
    [SerializeField] private float _stoppingDistance;

    [Header("Shake Amount")]
    [SerializeField] private float _chaseShakeAmount = 0.2f;
    [SerializeField] private float _shakeDelay = 1.0f;

    private Vector2 _targetPos;
    private float _shakeTimer;

    public override void Enter()
    {
        if (_targetPos == null)
        {
            Debug.Log("Enemy missing target");
            return;
        }

        _shakeTimer = 0f;
    }

    public override void Do()
    {
        if (Mathf.Abs(core.transform.position.x - _targetPos.x) > _stoppingDistance)
        {
            // Enemy chasing to target
            Vector2 dir = _targetPos - (Vector2)core.transform.position;
            _movement.SetHorizontalMovement(dir, _chaseSpeed);
            _shakeTimer += Time.deltaTime;
            if (_shakeTimer >= _shakeDelay)
            {
                _shakeTimer = 0;
                SoundFXManager.instance.PlaySoundFXClip(_footstepAudioClip, core.transform, 2f, regulated: false, randPitch: true);
                _impulseSource.GenerateImpulse(_chaseShakeAmount);
            }
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
