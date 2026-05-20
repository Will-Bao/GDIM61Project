using UnityEngine;

public class BookPickup : MonoBehaviour
{
    private PlayerBookHandler _playerBookHandler;
    private InputManager _inputManager;
    private bool _playerInRange;
    [SerializeField] private LevelMarker _marker;
    [SerializeField] private GameObject _toolTip;

    private void OnTriggerEnter2D(Collider2D other)
    {
        _playerBookHandler = other.GetComponentInParent<PlayerBookHandler>();
        _inputManager = other.GetComponentInParent<InputManager>();

        if (_playerBookHandler != null && _inputManager != null)
        {
            if (_marker != null && LevelManager.Instance.CurrentLevel != _marker.CurrentLayer) return;
            _playerInRange = true;
            _toolTip.SetActive(true);

        }
    }
    private void Awake()
    {
    if (_marker == null)
        _marker = GetComponent<LevelMarker>();
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerBookHandler playerBookHandler = other.GetComponentInParent<PlayerBookHandler>();
        if (playerBookHandler == _playerBookHandler)
        {
            _playerInRange = false; 
            _playerBookHandler = null;
            _inputManager = null;
            _toolTip.SetActive(false);

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