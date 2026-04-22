using UnityEngine;
using static TreeEditor.TreeEditorHelper;

public enum NoiseType
{
    Player,
    Item
}
public class NoiseData
{
    public Vector3 Location { get; private set; }
    public int Level;
    public int Layer;
    public NoiseType Type { get; private set; }

    public NoiseData(Vector3 location, int level, int layer, NoiseType type)
    {
        Location = location;
        Level = level;
        Layer = layer;
        Type = type;
    }
}
