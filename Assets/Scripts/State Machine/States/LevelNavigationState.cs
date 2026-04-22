using UnityEngine;

public class LevelNavigationState : State
{
    [Header("States")]
    [SerializeField] private EnemyChaseState _chase;
    [SerializeField] private AnimationState _idle;

    [Header("Components")]
    [SerializeField] private LevelTracker _tracker;

    [Header("Navigation")]
    [SerializeField] private float _stoppingDistance = 0.1f; // For level entrance/exit

    private int _targetLayer = -1;
    private Vector2 _targetLocation;

    public override void Enter()
    {
        if (_targetLayer < 0 || _tracker.CurrentLayer == _targetLayer)
        {
            isComplete = true;
        }
    }

    public override void Do()
    {
        if (_tracker.CurrentLayer == _targetLayer)
        {
            _chase.SetTarget(_targetLocation);
            machine.Set(_chase);
            if (machine.state.isComplete) isComplete = true;
            return;
        }

        Vector2 targetPos = (_tracker.CurrentLayer > _targetLayer) ? _tracker.Data.GetStart() : _tracker.Data.GetExit();
        if (Mathf.Abs(core.transform.position.x - targetPos.x) > _stoppingDistance)
        {
            _chase.SetTarget(targetPos);
            machine.Set(_chase);
        }
        else
        {
            machine.Set(_idle);
            int shift = (_tracker.CurrentLayer > _targetLayer) ? -1 : 1;
            _tracker.TransitionNewLayer(shift);
        }
    }

    public void SetTargetLayer(int layer, Vector2 location)
    {
        _targetLayer = layer;
        _targetLocation = location;
    }
}
