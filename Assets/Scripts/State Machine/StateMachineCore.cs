using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The base class for all entities with a state machine.
/// </summary>
public abstract class StateMachineCore : MonoBehaviour
{
    [Header("Core Components")]
    public Rigidbody2D rb;
    public Animator anim;
    [Header("Debug")]
    [SerializeField] private bool _debug;

    private float _facingDirection;
    public float FacingDirection => _facingDirection;
    protected StateMachine machine;

    /// <summary>
    /// Creates a state machine and assigns the core for each state.
    /// </summary>
    public void SetupInstances()
    {
        machine = new StateMachine();

        State[] allChildStates = GetComponentsInChildren<State>();
        foreach (State state in allChildStates)
        {
            state.SetCore(this);
        }
    }

    /// <summary>
    /// Flips the gameobject based on the movement direction.
    /// </summary>
    // Add to Update in inherited classes
    protected void SetFacingDirection()
    {
        // Returns -1, 0, or 1 for horizontal facing direction
        if (Mathf.Abs(rb.linearVelocityX) > 0.1f)
        {
            _facingDirection = Mathf.Sign(rb.linearVelocityX);
        }
        else if (_facingDirection == 0)
        {
            _facingDirection = 1f;
        }

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * _facingDirection;
        transform.localScale = scale;
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
        if (!_debug) return;
        if (machine == null) return;
        if (machine.state == null) return;
        
        List<State> states = machine.GetActiveStatesBranch();
        
        UnityEditor.Handles.Label(transform.position, "Active States: " + string.Join(" > ", states));
#endif
    }
}
