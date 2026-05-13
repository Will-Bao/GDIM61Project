using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class BeatGameManager : MonoBehaviour
{
    public static BeatGameManager Instance;
    [Header("Components")]
    [SerializeField] private GameObject _beatPrefab;
    [SerializeField] private Animator _anim;
    [SerializeField] private Animator _screenAnim;
    [SerializeField] private GameObject _beatParent;
    [SerializeField] private GameObject _heartObject;

    [Header("Audio")]
    [SerializeField] private AudioClip _beatClip;

    [Header("Settings")]
    [SerializeField] private float _travelTime;
    [SerializeField] private float _delay;
    [SerializeField] private float _hitWindow;
    [SerializeField] private float _xOffset;
    [SerializeField] private int _attemptsNum;

    [Header("Delay")]
    [SerializeField] private float _startDelay;
    [SerializeField] private float _endDelay;

    [Header("Difficulty")]
    [SerializeField] private float _delayMult;
    [SerializeField] private float _minDelay;

    private bool _gameStarted;
    private RectTransform _rect;

    private float _gameTimer;
    private float _lastBeatTime;
    private float _duration;
    private int _currentAttempts;
    private List<BeatObject> _activeBeats = new();

    private Player _player;
    private MinigameInput _minigameInput;
    private float _lastInput;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(this);
        _rect = GetComponent<RectTransform>();
        _heartObject.SetActive(false);
    }

    private void Start()
    {
        SetupPlayerReferences();
    }

    private void SetupPlayerReferences()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj == null) return;
        _player = playerObj.GetComponentInParent<Player>();
        _minigameInput = _player.GetComponent<MinigameInput>();
    }

    public void StartGame(float duration)
    {
        if (_gameStarted) return;
        if (_player == null)
        {
            SetupPlayerReferences();
        }
        _gameStarted = true;
        _gameTimer = 0;
        _duration = duration;
        _currentAttempts = _attemptsNum;
        _heartObject.SetActive(true);
    }

    public void NotifyMiss(BeatObject beat)
    {
        Beat data = beat.GetData();
        _activeBeats.Remove(beat);
        _anim.SetTrigger("OnBeatMiss");
        _screenAnim.SetTrigger("Shake");
        SoundFXManager.instance.PlaySoundFXClip(_beatClip, transform, 1f, regulated:false, randPitch: true);

        string direction = data.direction == -1 ? "Left" : "Right";
        Debug.Log("Missed " + direction + " Beat");

        _currentAttempts--;

        if (_currentAttempts <= 0)
        {
            Debug.Log("Player lost minigame");
            _player.ToggleHiding(false);
            EndGame();
        }
    }

    public void TryHit(float direction)
    {
        BeatObject nearest = GetNearestBeat(direction);
        if (nearest == null) return;

        float error = Mathf.Abs(Time.time - nearest.GetData().hitTime);
        if (error <= _hitWindow)
        {
            Hit(nearest);
            Debug.Log($"Hit error: {error}");
        }
    }

    private void Hit(BeatObject beat)
    {
        Beat data = beat.GetData();
        _activeBeats.Remove(beat);
        _anim.SetTrigger("OnBeatHit");
        SoundFXManager.instance.PlaySoundFXClip(_beatClip, transform, 0.5f, regulated: false, randPitch: true);

        string direction = data.direction == -1 ? "Left" : "Right";
        Debug.Log("Hit " + direction + " Beat");

        Destroy(beat.gameObject);
    }

    private void Update()
    {
        if (!_gameStarted) return;
        if (!_player.IsHidden) EndGame();
        // Player input
        HandleInput();
        // Game logic
        _gameTimer += Time.deltaTime;

        if (_gameTimer < _startDelay) return;
        
        if (_gameTimer + _travelTime < _duration + _startDelay && Time.time - _lastBeatTime > GetDelay())
        {
            SpawnBeat();
            _lastBeatTime = Time.time;
        }
        else if (_gameTimer >= _duration + _endDelay + _startDelay)
        {
            EndGame();
        }
    }

    private void HandleInput()
    {
        float currentX = _minigameInput.XInput;
        if (_lastInput == 0 && currentX < 0)
        {
            TryHit(-1);
        }
        else if (_lastInput == 0 && currentX > 0)
        {
            TryHit(1);
        }
        _lastInput = currentX;
    }

    private void SpawnBeat()
    {
        // get random direction (1 = Right / -1 = Left)
        float beatDirection = Random.Range(0, 2) == 0 ? -1 : 1;
        Beat data = new Beat(Time.time + _travelTime, _travelTime, beatDirection);

        GameObject beatObject = Instantiate(_beatPrefab, _beatParent.transform);
        RectTransform beatRect = beatObject.GetComponent<RectTransform>();
        beatRect.anchoredPosition = new Vector2(_rect.anchoredPosition.x + _xOffset * beatDirection, 0);

        BeatObject beat = beatObject.GetComponent<BeatObject>();
        beat.Initialize(data, _rect.anchoredPosition, _xOffset * beatDirection);

        Debug.Log($"Beat spawned at {_rect.anchoredPosition}, Target pos {beatRect.anchoredPosition}");

        _activeBeats.Add(beat);
    }

    private void EndGame()
    {
        if (!_gameStarted) return;
        _gameStarted = false;
        _heartObject.SetActive(false);
        ClearBeats();
        MinigameManager.Instance.EndGame();
    }

    private void ClearBeats()
    {
        foreach (var beat in _activeBeats)
        {
            Destroy(beat.gameObject);
        }
    }

    /// <summary>
    /// Get the beat object with the closest hitTime in the specfied direction.
    /// </summary>
    /// <param name="direction"> Hit direction. </param>
    /// <returns> The closest Beat Object </returns>
    private BeatObject GetNearestBeat(float direction)
    {
        return _activeBeats.Where(n => n.GetData().direction == direction)
                           .OrderBy(n => Mathf.Abs(Time.time - n.GetData().hitTime)).FirstOrDefault();
    }

    private float GetDelay()
    {
        float delay = _delay / (1 + MinigameManager.Instance.TotalGameAmount * _delayMult);
        return Mathf.Max(delay, _minDelay);
    }
}
