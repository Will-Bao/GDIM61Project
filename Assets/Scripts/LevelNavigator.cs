using UnityEngine;
using UnityEngine.InputSystem;

public class LevelNavigator : MonoBehaviour
{
    [SerializeField] private InputManager _input;

    private bool _isNearExit;
    public void EnterNextLevel()
    {
        // TODO: Add level switching here
        SetPlayerWin();
    }

    public void SetPlayerWin()
    {
        GameManager.Instance.PlayerWin();
    }

    private void Update()
    {
        if (_isNearExit && _input.MoveInput.y > 0 )
        {
            EnterNextLevel();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Exit"))
        {
            _isNearExit = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Exit"))
        {
            _isNearExit = false;
        }
    }
}
