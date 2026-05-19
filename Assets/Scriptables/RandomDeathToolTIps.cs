using UnityEngine;

[CreateAssetMenu(fileName = "Deathtooltips", menuName = "Scriptable Objects/Deathtooltips")]
public class Deathtooltips : ScriptableObject
{
    [TextArea(2, 5)]
    public string[] tooltipLines;

    public string GetRandomLine()
    {
        if (tooltipLines == null || tooltipLines.Length == 0)
        {
            return "";
        }

        int randomIndex = Random.Range(0, tooltipLines.Length);
        return tooltipLines[randomIndex];
    }
}
