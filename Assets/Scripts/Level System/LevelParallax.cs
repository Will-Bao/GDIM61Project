using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelParallax : MonoBehaviour
{
    [Header("Layer")]
    [SerializeField] private int _layerNum;
    [SerializeField] private int _sortingOffsetAmount = 2;
    [SerializeField] private float _parallaxAmount = 1f;
    [SerializeField] private float _transitionTime = 0.4f;

    public int LayerNum => _layerNum;

    private Dictionary<SpriteRenderer, int> _originalOrders = new();
    private Dictionary<SpriteRenderer, Color> _originalColors = new();
    private Coroutine _transitionRoutine;
    private Coroutine _targetRoutine;

    private float _darkenPercentage = 0.7f;

    private void Awake()
    {
        foreach (SpriteRenderer renderer in GetComponentsInChildren<SpriteRenderer>())
        {
            _originalOrders[renderer] = renderer.sortingOrder;
            _originalColors[renderer] = renderer.color;
        }
    }
    private void Start()
    {
        ApplyParallaxLayer();
    }

    public void SetParallaxLayer(int layer)
    {
        _layerNum = layer;
        if (layer < 0)
        {
            gameObject.SetActive(false);
            return;
        }
        else gameObject.SetActive(true);

        if (_transitionRoutine != null) StopCoroutine(_transitionRoutine);

        _transitionRoutine = StartCoroutine(ParallaxTransition(transform, layer, _originalOrders, _originalColors));
    }

    public void SetObjectLayer(Transform target, int layer, Dictionary<SpriteRenderer, int> orders,
                               Dictionary<SpriteRenderer, Color> colors)
    {
        if (layer < 0) return; // handled layer < 0 on LayerTracker
        if (_targetRoutine != null) StopCoroutine(_targetRoutine);

        _targetRoutine = StartCoroutine(ParallaxTransition(target, layer, orders, colors));
    }

    private IEnumerator ParallaxTransition(Transform target, int layer, Dictionary<SpriteRenderer, int> orders,
                                           Dictionary<SpriteRenderer, Color> colors)
    {
        float elapsed = 0f;

        Vector3 startPos = target.position;
        float targetZ = layer * _parallaxAmount;
        Vector3 targetPos = new Vector3(startPos.x, startPos.y, targetZ);

        var renderers = target.GetComponentsInChildren<SpriteRenderer>();

        Dictionary<SpriteRenderer, int> startOrders = new();
        Dictionary<SpriteRenderer, int> targetOrders = new();
        Dictionary<SpriteRenderer, Color> startColors = new();
        Dictionary<SpriteRenderer, Color> targetColors = new();

        // Transition
        foreach (var r in renderers)
        {
            if (!orders.ContainsKey(r)) orders[r] = r.sortingOrder;

            if (!colors.ContainsKey(r)) colors[r] = r.color;

            startOrders[r] = r.sortingOrder;
            targetOrders[r] = orders[r] - layer * _sortingOffsetAmount;

            startColors[r] = r.color;

            float darkenFactor = Mathf.Clamp01(1f - (layer * _darkenPercentage));
            Color baseColor = colors[r];

            targetColors[r] = new Color(baseColor.r * darkenFactor, baseColor.g * darkenFactor,
                                        baseColor.b * darkenFactor, baseColor.a);
        }

        while (elapsed < _transitionTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / _transitionTime);

            target.position = Vector3.Lerp(startPos, targetPos, t);

            foreach (var r in renderers)
            {
                r.sortingOrder = Mathf.RoundToInt(Mathf.Lerp(startOrders[r], targetOrders[r], t));
                r.color = Color.Lerp(startColors[r], targetColors[r], t);
            }

            yield return null;
        }

        // Final
        target.position = targetPos;

        foreach (var r in renderers)
        {
            r.sortingOrder = targetOrders[r];
            r.color = targetColors[r];
        }
    }

    private void ApplyParallaxLayer()
    {
        // Shift z position
        Vector3 currentPos = transform.position;
        currentPos.z = _layerNum * _parallaxAmount;
        transform.position = currentPos;

        // Adjust sorting layer
        foreach (SpriteRenderer renderer in GetComponentsInChildren<SpriteRenderer>())
        {
            renderer.sortingOrder = _originalOrders[renderer] - _layerNum * _sortingOffsetAmount;

            float darkenFactor = 1f - (_layerNum * _darkenPercentage);
            darkenFactor = Mathf.Clamp01(darkenFactor);

            Color baseColor = _originalColors[renderer];

            renderer.color = new Color(baseColor.r * darkenFactor, baseColor.g * darkenFactor,
                                       baseColor.b * darkenFactor, baseColor.a);
        }
    }
}
