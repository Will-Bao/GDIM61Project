using System;
using Unity.Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : StateMachineCore
{
    [Header("States")]
    [SerializeField] private EnemyPatrolState _patrol;
    [SerializeField] private EnemyChaseState _chase;
    [SerializeField] private LevelNavigationState _levelNav;
    [SerializeField] private EnemyAttackState _attack;
    [SerializeField] private AnimationState _idle;

    [Header("Components")]
    [SerializeField] private CinemachineImpulseSource _cameraImpulse;
    [SerializeField] private LevelTracker _levelTracker;

    [Header("Player Detection")]
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private float _detectionRange;

    [Header("Proximity Shake")]
    [SerializeField] private float _shakeAmount;
    [SerializeField] private float _shakeDelay;

    [Header("Proximity Audio")]
    [SerializeField] private AudioClip _proximityAudio;

    [Header("Random Teleport")]
    [SerializeField] private float _teleportChance;
    [SerializeField] private float _checkDelay;

    private Vector2 _targetPos;
    private float _shakeTimer = 0f;
    private float _teleportTimer = 0f;

    private void Start()
    {
        SetupInstances();
        machine.Set(_patrol, true);
    }

    private void Update()
    {
        ToggleVisibility(_levelTracker.CurrentLayer >= LevelManager.Instance.CurrentLevel);
        TeleportCheck();
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
        if (MinigameManager.Instance.GameStarted) return;
        if (CheckForPlayer())
        {
            GameManager.Instance.SetPlayerSpotted(true);
            _chase.SetTarget(_targetPos);
            machine.Set(_chase);
        }
        else
        {
            GameManager.Instance.SetPlayerSpotted(false);
            machine.Set(_patrol);
        }
    }

    public void AlertEnemy(NoiseData noise)
    {
        if (noise.Type == NoiseType.Player) GameManager.Instance.SetPlayerSpotted(true);
        if (IsOnSameLayer())
        {
            _targetPos = noise.Location;
            _chase.SetTarget(noise.Location);
            machine.Set(_chase);
        }
        else
        {
            _levelNav.SetTargetLayer(noise.Layer, noise.Location);
            machine.Set(_levelNav);
        }
    }

    public void ToggleVisibility(bool visible)
    {
        anim.GetComponent<SpriteRenderer>().enabled = visible;
    }

    private bool CheckForPlayer()
    {
        Collider2D[] detectedColliders = Physics2D.OverlapCircleAll(transform.position, _detectionRange, _playerLayer);
        foreach (var detected in detectedColliders)
        {
            if (detected.CompareTag("Player") && IsOnSameLayer())
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
        return (machine.state == _chase || machine.state == _levelNav) && (!machine.state.isComplete);
    }

    private bool IsAttacking()
    {
        return (machine.state == _attack) && (!machine.state.isComplete);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && IsOnSameLayer())
        {
            Player player = collision.GetComponentInParent<Player>();
            if (!player.IsHidden)
            {
                machine.Set(_attack);
                player.SetPlayerDead();
            }
            else if (IsChasing() && !MinigameManager.Instance.GameStarted &&
                     Mathf.Abs(_targetPos.x - transform.position.x) < 0.1f)
            {
                MinigameManager.Instance.StartGame(MinigameManager.GameType.Random);
                machine.Set(_idle);
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
            SoundFXManager.instance.PlaySoundFXClip(_proximityAudio, transform, 8f, regulated: false, randPitch: true);
        }
    }

    private bool IsOnSameLayer()
    {
        return _levelTracker.CurrentLayer == LevelManager.Instance.CurrentLevel;
    }

    private void TeleportCheck()
    {
        if (_levelTracker.CurrentLayer >= LevelManager.Instance.CurrentLevel ||
            LevelManager.Instance.CurrentLevel == LevelManager.Instance.GetLastLevel())
        {
            _teleportTimer = 0f;
            return;
        }
        else
        {
            _teleportTimer += Time.deltaTime;
        }

        if (_teleportTimer >= _checkDelay)
        {
            _teleportTimer = 0f;
            if (Random.value > _teleportChance) return;
            // Teleport
            int targetLayer = Random.Range(LevelManager.Instance.CurrentLevel + 1, LevelManager.Instance.GetLastLevel());
            int shiftAmount = targetLayer - _levelTracker.CurrentLayer;
            _levelTracker.TransitionNewLayer(shiftAmount);
            Debug.Log($"Enemy teleported to layer {targetLayer}");
        }
    }
    public void ForceToCurrentPlayerLayer()
    {
        int shiftAmount = LevelManager.Instance.CurrentLevel - _levelTracker.CurrentLayer;
        _levelTracker.TransitionNewLayer(shiftAmount);
    }
}
