using UnityEngine;

public class PlayerStunnedState : State
{
    [Header("Components")]
    [SerializeField] private MovementHandler _movement;

    public override void Enter()
    {
        _movement.SetCanMove(false);
    }

    public override void Exit()
    {
        _movement.SetCanMove(true);
    }
}
