using UnityEngine;

/// <summary>
/// Marks what layer an object is in.
/// This object shouldn't be able to move to different layers.
/// </summary>
public class LevelMarker : MonoBehaviour
{
    public int CurrentLayer { get; private set; }

    private void Start()
    {
        CurrentLayer = GetComponentInParent<LevelParallax>().LayerNum;
    }
    public void SetCurrentLayer(int newLayer)
    {
        CurrentLayer = newLayer;
    }
}
