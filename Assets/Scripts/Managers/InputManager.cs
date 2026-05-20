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
    public InputAction CrouchAction;
    public Vector2 MoveInput { get; private set; }
    public bool CrouchInput { get; private set; }
    public bool InteractPressed => InteractAction.WasPressedThisFrame();
    private void OnEnable()
    {
        MoveAction.Enable();
        InteractAction.Enable();
        CrouchAction.Enable();

        MoveAction.performed += OnMoveInput;
        MoveAction.canceled += OnMoveInput;
        CrouchAction.performed += OnCrouchPerformed;
        CrouchAction.canceled += OnCrouchCanceled;
    }

    private void OnDisable()
    {
        MoveAction.performed -= OnMoveInput;
        MoveAction.canceled -= OnMoveInput;
        CrouchAction.performed -= OnCrouchPerformed;
        CrouchAction.canceled -= OnCrouchCanceled;


        MoveAction.Disable();
        InteractAction.Disable();
        CrouchAction.Disable();
    }

    public void OnMoveInput(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
    }

    public void OnCrouchPerformed(InputAction.CallbackContext ctx)
    {
        CrouchInput = ctx.performed;
    }

    public void OnCrouchCanceled(InputAction.CallbackContext ctx)
    {
        CrouchInput = ctx.performed;
    }
}
