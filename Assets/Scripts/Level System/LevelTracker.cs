using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class LevelTracker : MonoBehaviour
{
    [Header("Layer")]
    [SerializeField] private int _startingLayer = 1;

    private LevelData _levelData;
    private LevelParallax _layer;
    private int _currentLayer;

    void Start()
    {
        _layer = LevelManager.Instance.GetLevelParallax(_startingLayer);
        if (_layer != null) _currentLayer = _layer.LayerNum;
        else Debug.LogWarning("Enemy not assigned to a layer");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
