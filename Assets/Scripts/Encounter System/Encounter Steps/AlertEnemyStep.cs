using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Encounter/Steps/Alert Enemy")]
public class AlertEnemyStep : EncounterStep
{
    [Header("Settings")]
    [SerializeField] private int _noiseLevel;

    public override IEnumerator Execute(EncounterContext context)
    {
        NoiseManager.Instance.CreateNoise(new NoiseData(context.Location, _noiseLevel, context.Layer, NoiseType.Player));
        yield break;
    }
}
