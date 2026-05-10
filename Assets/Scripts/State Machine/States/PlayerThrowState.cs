using UnityEngine;

public class PlayerThrowState : State
{
    [Header("Animation")]
    [SerializeField] private AnimationClip _throwAnim;
    [Header("Throw Strength")]
    [SerializeField] private float _throwUpwardForce = 0.75f;
    [SerializeField] private float _throwDuration = 0.15f;

    private Player _player;
    private bool _hasThrown;

    public override void Enter()
    {
        _player = core as Player;
        _hasThrown = false;
        isComplete = false;
    }

    public override void Do()
    {
        if (_player == null)
        {
            isComplete = true;
            return;
        }

        if (!_hasThrown)
        {
            float xDirection = _player.FacingDirection;
            if (xDirection == 0) xDirection = 1f;

            _player.BookHandler.ThrowBook(new Vector2(xDirection, _throwUpwardForce));
            _hasThrown = true;
        }

        if (time >= _throwDuration)
        {
            isComplete = true;
        }
    }
}