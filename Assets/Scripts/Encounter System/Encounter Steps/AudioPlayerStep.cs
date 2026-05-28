using System.Collections;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(menuName = "Encounter/Steps/Audio Player")]
public class AudioPlayerStep : EncounterStep
{
    [Header("Audio Settings")]
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private float volume = 1f;
    [SerializeField] private bool isRegulated = false;
    [SerializeField] private bool isPitched = true;
    public override IEnumerator Execute(EncounterContext context)
    {
        SoundFXManager.instance.PlaySoundFXClip(_audioClip, context.Player, volume, regulated: isRegulated, randPitch: isPitched);
        yield break;
    }
}
