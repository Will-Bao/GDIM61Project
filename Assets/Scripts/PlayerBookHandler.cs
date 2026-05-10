using UnityEngine;

public class PlayerBookHandler : MonoBehaviour
{
    [SerializeField] private bool _hasBook;
    [SerializeField] private float _throwOffsetX = 0.5f;
    [SerializeField] private float _throwOffsetY = 0.2f;
    [SerializeField] private GameObject _thrownBookPrefab;
    [SerializeField] private float _throwForce = 8f;
    [SerializeField] private Player _player;
    [SerializeField] private InputManager _inputManager;
    public bool HasBook => _hasBook;
    private bool _canThrow;
    
    public void GainBook()
    {
        _hasBook = true;
        _canThrow = false;
    }
    public void ThrowBook(Vector2 direction)
    {
        if (!_hasBook) return;
        if (!_thrownBookPrefab) return;
        if (direction == Vector2.zero)
        {
            direction = new Vector2(1f, 1f);
        }
        float facing = direction.x >= 0 ? 1f : -1f;
        Vector3 spawnPosition = transform.position + new Vector3(_throwOffsetX * facing, _throwOffsetY, 0f);
        GameObject thrownBook = Instantiate(_thrownBookPrefab, spawnPosition, Quaternion.identity);
        Rigidbody2D rb = thrownBook.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction.normalized * _throwForce, ForceMode2D.Impulse);

        _hasBook = false;    
    }
    private void Update()
    {
        if (!_hasBook) return;
        if (_inputManager == null) return;

        if (!_inputManager.InteractPressed)
        {
            _canThrow = true;
            return;
        }

        if (!_canThrow) return;

        float xDirection = _player.transform.localScale.x > 0 ? 1f : -1f;
        ThrowBook(new Vector2(xDirection, 0.75f));
        _canThrow = false;
    }

}
