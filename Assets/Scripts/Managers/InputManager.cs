using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Processes the player's inputs into public values.
/// </summary>
public class InputManager : MonoBehaviour
{
    [Header("Input actions")]
    public InputAction MoveAction;
    public InputAction InteractAction;
    public Vector2 MoveInput { get; private set; }
    public bool InteractPressed => InteractAction.WasPressedThisFrame();
    private void OnEnable()
    {
        MoveAction.Enable();
        InteractAction.Enable();

        MoveAction.performed += OnMoveInput;
        MoveAction.canceled += OnMoveInput;
    }

    private void OnDisable()
    {
        MoveAction.performed -= OnMoveInput;
        MoveAction.canceled -= OnMoveInput;

        MoveAction.Disable();
        InteractAction.Disable();
    }

    public void OnMoveInput(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
    }
}
