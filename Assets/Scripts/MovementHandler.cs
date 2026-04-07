using UnityEngine;

/// <summary>
/// Handles an entity's velocity-based movement.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class MovementHandler : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Vector2 _velocity;
    private bool _canMove = true;

    /// <summary>
    /// Set the entity's x velocity movement.
    /// </summary>
    /// <param name="direction"> Direction to move toward. </param>
    /// <param name="speed"> Movement speed. </param>
    public void SetHorizontalMovement(Vector2 direction, float speed)
    {
        if (!_canMove)
        {
            _velocity = Vector2.zero;
            return;
        }
        _velocity.x = (direction * speed).x;
    }

    /// <summary>
    /// Sets the entity's ability to move.
    /// </summary>
    /// <param name="canMove"> True if entity can move, false otherwise. </param>
    public void SetCanMove(bool canMove)
    {
        _canMove = canMove;
        if (canMove == false) _velocity = Vector2.zero;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        if (_canMove)
        {
            _rb.linearVelocity = new Vector2(_velocity.x, _rb.linearVelocity.y);
        }
    }
}
