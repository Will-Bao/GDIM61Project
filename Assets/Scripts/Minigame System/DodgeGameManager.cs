using System.Collections;
using UnityEngine;

public class DodgeGameManager : MonoBehaviour
{
    public static DodgeGameManager Instance;

    [Header("Components")]
    [SerializeField] private GameObject _tongueObj;
    [SerializeField] private GameObject _heartObj;
    [SerializeField] private Animator _screenAnim;
    [SerializeField] private Animator _heartAnim;

    [Header("Animations")]
    [SerializeField] private AnimationClip _extendAnim;
    [SerializeField] private AnimationClip _retractAnim;

    [Header("Attack Settings")]
    [SerializeField] private float _xBoundarySize;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _delay;
    [SerializeField] private float _attackDuration;
    [SerializeField] private float _targetPlayerChance = 0.5f;

    [Header("Heart Settings")]
    [SerializeField] private float _moveSpeed;

    [Header("Delay")]
    [SerializeField] private float _startDelay;
    [SerializeField] private float _endDelay;

    [Header("Difficulty")]
    [SerializeField] private float _delayMult;
    [SerializeField] private float _minDelay;
    [SerializeField] private int _attemptsNum = 4;

    private bool _gameStarted;

    private float _gameTimer;
    private float _gameDuration;
    private float _lastSpawnTime;
    private int _currentAttempts;

    private float _centerX;
    private RectTransform _heartRect;

    private Player _player;
    private MinigameInput _minigameInput;
    private Animator _tongueAnim;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(this);
        _centerX = _tongueObj.transform.position.x;
        _tongueObj.SetActive(false);
        _heartObj.SetActive(false);
    }
    private void Start()
    {
        SetupPlayerReferences();
        _tongueAnim = _tongueObj.GetComponent<Animator>();
        _heartRect = _heartObj.GetComponent<RectTransform>();
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
        _currentAttempts = _attemptsNum;
        _gameDuration = duration;
        _tongueObj.SetActive(true);
        _heartObj.SetActive(true);
        _heartRect.anchoredPosition = new Vector2(_centerX, _heartRect.anchoredPosition.y);

        // Rebind anim
        _tongueAnim.Rebind();
        _tongueAnim.Update(0f);
    }

    private void Update()
    {
        if (!_gameStarted) return;
        if (!_player.IsHidden) EndGame();
        // Player input
        if (_gameTimer < _gameDuration + _startDelay) MoveHeart();
        // Game logic
        _gameTimer += Time.deltaTime;
        if (_gameTimer < _startDelay) return;

        if (_gameTimer + _attackDuration < _gameDuration + _startDelay &&
            Time.time - _lastSpawnTime > GetDelay() + _attackDuration)
        {
            StartCoroutine(SpawnTongue());
            _lastSpawnTime = Time.time;
        }
        else if (_gameTimer >= _gameDuration + _endDelay + _startDelay)
        {
            EndGame();
        }
    }

    private void MoveHeart()
    {
        Vector2 pos = _heartRect.anchoredPosition;

        pos.x += _minigameInput.XInput * _moveSpeed * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, _centerX - _xBoundarySize, _centerX + _xBoundarySize);

        _heartRect.anchoredPosition = pos;
    }

    private IEnumerator SpawnTongue()
    {
        // STARTUP
        // Set new spawn location
        float spawnX = GetSpawnLocation();
        RectTransform tongueRect = _tongueObj.GetComponent<RectTransform>();
        tongueRect.anchoredPosition = new Vector2(spawnX, tongueRect.anchoredPosition.y);

        // Set anim
        _tongueAnim.speed = _extendAnim.length / (_attackDuration / 2);
        _tongueAnim.Play(_extendAnim.name);

        yield return new WaitForSeconds(_attackDuration / 2);

        if (Mathf.Abs(_heartRect.anchoredPosition.x - spawnX) < _attackRange) Hit();

        // ENDING
        _tongueAnim.speed = _retractAnim.length / (_attackDuration / 2);
        _tongueAnim.Play(_retractAnim.name);

        yield return new WaitForSeconds(_attackDuration / 2);
    }

    private float GetSpawnLocation()
    {
        if (Random.Range(0f, 1f) <= _targetPlayerChance)
        {
            // Target player
            Debug.Log("Targetting heart directly");
            return _heartRect.anchoredPosition.x;
        }
        else
        {
            // Random pos
            return Random.Range(-_xBoundarySize, _xBoundarySize) + _centerX;
        }
    }

    private void Hit()
    {
        Debug.Log("Tongue hit player");
        _currentAttempts--;
        _screenAnim.SetTrigger("Shake");
        _heartAnim.SetTrigger("OnHit");

        if (_currentAttempts <= 0)
        {
            Debug.Log("Player lost minigame");
            _player.ToggleHiding(false);
            EndGame();
        }
    }

    private void EndGame()
    {
        if (!_gameStarted) return;
        _gameStarted = false;
        StopAllCoroutines();
        _tongueObj.SetActive(false);
        _heartObj.SetActive(false);
        MinigameManager.Instance.EndGame();
    }

    private float GetDelay()
    {
        float delay = _delay / (1 + MinigameManager.Instance.TotalGameAmount * _delayMult);
        return Mathf.Max(delay, _minDelay);
    }
}
