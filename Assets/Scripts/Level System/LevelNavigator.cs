using UnityEngine;
using UnityEngine.InputSystem;

public class LevelNavigator : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private InputManager _input;
    [SerializeField] private Player _player;
    [SerializeField] private GameObject _nextIndicator;
    [SerializeField] private GameObject _backIndicator;

    private bool _isNearExit;
    private bool _isNearEntrance;

    public void SetPlayerWin()
    {
        GameManager.Instance.PlayerWin();
    }

    private void Update()
    {
        UpdateIndicators();
        if (_player.IsDead) return;
        if (_isNearExit && _input.MoveInput.y > 0)
        {
            LevelManager.Instance.NextLevel();
            _player.SetPlayerTransition(1);
            _isNearExit = false;
            if (LevelManager.Instance.CurrentLevel > 1) _isNearEntrance = true;
        }
        else if (_isNearEntrance && _input.MoveInput.y < 0)
        {
            LevelManager.Instance.PreviousLevel();
            _player.SetPlayerTransition(-1);
            _isNearEntrance = false;
            _isNearExit = true;
        }
    }

    private void UpdateIndicators()
    {
        _nextIndicator.SetActive(_isNearExit);
        _backIndicator.SetActive(_isNearEntrance);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out LevelMarker marker) ||
            marker.CurrentLayer != LevelManager.Instance.CurrentLevel) return; // Check for same layer

        if (collision.CompareTag("Exit"))
        {
            _isNearExit = true;
        }
        if (collision.CompareTag("Entrance"))
        {
            _isNearEntrance = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out LevelMarker marker) ||
            marker.CurrentLayer != LevelManager.Instance.CurrentLevel) return; // Check for same layer

        if (collision.CompareTag("Exit"))
        {
            _isNearExit = false;
        }
        if (collision.CompareTag("Entrance"))
        {
            _isNearEntrance = false;
        }
    }
}
