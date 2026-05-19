using UnityEngine;
using TMPro;

public class DeathToolTIpSystem : MonoBehaviour
{
    [SerializeField] private DialogueSystems _dialogueSystem;

    [SerializeField] private Deathtooltips _deathTooltips;
    private void Start()
    {
        PlayRandomDeathTooltip();
    }
    public void PlayRandomDeathTooltip()
    {
        string randomLine = _deathTooltips.GetRandomLine();

        _dialogueSystem.StartSingleLine(randomLine);
    }
}
