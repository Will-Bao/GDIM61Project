using System.Collections.Generic;
using UnityEngine;
public class LevelTracker : MonoBehaviour
{
    [Header("Layer")]
    [SerializeField] private int _startingLayer = 1;

    public int CurrentLayer { get; private set; }
    public LevelData Data { get; private set; }
    private LevelParallax _layer;

    private Dictionary<SpriteRenderer, int> _originalOrders = new();
    private Dictionary<SpriteRenderer, Color> _originalColors = new();

    private void Start()
    {
        _layer = LevelManager.Instance.GetLevelParallax(_startingLayer);
        Data = LevelManager.Instance.GetLevelData(_startingLayer);
        CurrentLayer = _layer.LayerNum;
        CacheOriginals(GetComponentsInChildren<SpriteRenderer>());
        _layer.SetObjectLayer(transform, CurrentLayer, _originalOrders, _originalColors);
    }

    private void OnEnable()
    {
        LevelManager.OnLevelSwitched += HandleGlobalTransition;
    }

    private void OnDisable()
    {
        LevelManager.OnLevelSwitched -= HandleGlobalTransition;
    }

    private void HandleGlobalTransition(int playerLayer)
    {
        int offset = CurrentLayer - playerLayer;

        var layer = LevelManager.Instance.GetLevelParallax(CurrentLayer);
        if (layer.gameObject.activeSelf) layer.SetObjectLayer(transform, offset, _originalOrders, _originalColors);
    }

    public void TransitionNewLayer(int shiftAmount)
    {
        CurrentLayer += shiftAmount;

        Data = LevelManager.Instance.GetLevelData(CurrentLayer);

        int playerLayer = LevelManager.Instance.CurrentLevel;
        int offset = CurrentLayer - playerLayer;

        var layer = LevelManager.Instance.GetLevelParallax(CurrentLayer);
        if (layer.gameObject.activeSelf) layer.SetObjectLayer(transform, offset, _originalOrders, _originalColors);
    }

    private void CacheOriginals(SpriteRenderer[] renderers)
    {
        foreach (var r in renderers)
        {
            if (!_originalOrders.ContainsKey(r))
                _originalOrders[r] = r.sortingOrder;

            if (!_originalColors.ContainsKey(r))
                _originalColors[r] = r.color;
        }
    }
}
