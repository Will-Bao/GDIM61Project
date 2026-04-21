using UnityEngine;
using UnityEngine.InputSystem;

public class LevelNavigator : MonoBehaviour
{
    [SerializeField] private InputManager _input;

    private bool _isNearExit;
    private bool _isNearEntrance;

    public void SetPlayerWin()
    {
        GameManager.Instance.PlayerWin();
    }

    private void Update()
    {
        if (_isNearExit && _input.MoveInput.y > 0)
        {
            LevelManager.Instance.NextLevel();
            _isNearExit = false;
            _isNearEntrance = true;
        }
        else if (_isNearEntrance && _input.MoveInput.y < 0)
        {
            LevelManager.Instance.PreviousLevel();
            _isNearEntrance = false;
            _isNearExit = true;
        }
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
