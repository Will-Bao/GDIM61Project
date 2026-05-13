using UnityEngine;

[CreateAssetMenu(fileName = "DialogueData", menuName = "Scriptable Objects/DialogueData")]
public class DialogueData : ScriptableObject
{
    public DialogueLine[] lines;
}
[System.Serializable]
public class DialogueLine
{
    [TextArea(2, 5)]
    public string text;
}