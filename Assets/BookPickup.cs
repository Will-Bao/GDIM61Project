using UnityEngine;

public class BookPickup : MonoBehaviour
{
    private PlayerBookHandler _playerBookHandler;
    private InputManager _inputManager;
    private bool _playerInRange;

    private void OnTriggerEnter2D(Collider2D other)
    {
        _playerBookHandler = other.GetComponentInParent<PlayerBookHandler>();
        _inputManager = other.GetComponentInParent<InputManager>();

        if (_playerBookHandler != null && _inputManager != null)
        {
            _playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerBookHandler playerBookHandler = other.GetComponentInParent<PlayerBookHandler>();

        if (playerBookHandler == _playerBookHandler)
        {
            _playerInRange = false;
            _playerBookHandler = null;
            _inputManager = null;
        }
    }

    private void Update()
    {
        if (!_playerInRange) return;
        if (_playerBookHandler == null) return;
        if (_inputManager == null) return;
        if (_playerBookHandler.HasBook) return;

        if (_inputManager.InteractPressed)
        {
            Debug.Log("Interact pressed, picking up book");
            _playerBookHandler.GainBook();
            Destroy(gameObject);
        }
    }
}