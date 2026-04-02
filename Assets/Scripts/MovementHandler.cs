using UnityEngine;

/// <summary>
/// Handles an entity's velocity-based movement.
/// </summary>
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
        _velocity = (direction * speed);
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
