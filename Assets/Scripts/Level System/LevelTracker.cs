using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class LevelTracker : MonoBehaviour
{
    [Header("Layer")]
    [SerializeField] private int _startingLayer = 1;

    public int CurrentLayer { get; private set; }
    private LevelData _levelData;
    private LevelParallax _layer;

    void Start()
    {
        _layer = LevelManager.Instance.GetLevelParallax(_startingLayer);
        _levelData = LevelManager.Instance.GetLevelData(_startingLayer);
        CurrentLayer = _layer.LayerNum;
        _layer.SetObjectLayer(transform, CurrentLayer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
