using UnityEngine;

public class LevelParallax : MonoBehaviour
{
    [Header("Layer")]
    [SerializeField] private int _layerNum;
    [SerializeField] private int _sortingOffsetAmount = 2;
    [SerializeField] private float _parallaxAmount = 1f;

    private void Start()
    {
        // Shift z position
        Vector3 currentPos = transform.position;
        currentPos.z = _layerNum * _parallaxAmount;
        transform.position = currentPos;

        // Adjust sorting layer
        foreach (SpriteRenderer renderer in GetComponentsInChildren<SpriteRenderer>())
        {
            renderer.sortingOrder -= _layerNum * _sortingOffsetAmount;

            float darkenFactor = 1f - (_layerNum * 0.1f);
            darkenFactor = Mathf.Clamp01(darkenFactor);

            Color c = renderer.color;
            c *= darkenFactor;
            renderer.color = c;
        }
    }
}
