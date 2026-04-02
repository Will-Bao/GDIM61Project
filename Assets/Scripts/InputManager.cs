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
    public bool Interact {  get; private set; }

    private void OnEnable()
    {
        MoveAction.Enable();
        InteractAction.Enable();

        MoveAction.performed += OnMoveInput;
        MoveAction.canceled += OnMoveInput;
        InteractAction.performed += OnInteract;
        InteractAction.canceled += OnInteract;
    }

    private void OnDisable()
    {
        MoveAction.performed -= OnMoveInput;
        MoveAction.canceled -= OnMoveInput;
        InteractAction.performed -= OnInteract;
        InteractAction.canceled -= OnInteract;

        MoveAction.Disable();
        InteractAction.Disable();
    }

    public void OnMoveInput(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        Interact = ctx.ReadValue<float>() == 1 ? true : false;
    }
}
