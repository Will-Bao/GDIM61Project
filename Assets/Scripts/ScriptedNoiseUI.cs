using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Encounter/Steps/Scripted Noise")]
public class ScriptedNoiseStep : EncounterStep
{
    [SerializeField] private int noiseLevel = 5;
    [SerializeField] private NoiseType noiseType = NoiseType.Player;

    public override IEnumerator Execute(EncounterContext context)
    {
        NoiseManager.Instance.CreateNoise(
            new NoiseData(
                context.Location,
                noiseLevel,
                context.Layer,
                noiseType
            )
        );

        yield return null;
    }
}