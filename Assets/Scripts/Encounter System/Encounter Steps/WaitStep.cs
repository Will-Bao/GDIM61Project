using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Encounter/Steps/Wait")]
public class WaitStep : EncounterStep
{
    [Header("Settings")]
    [SerializeField] private float duration;

    public override IEnumerator Execute(EncounterContext context)
    {
        yield return new WaitForSeconds(duration);
    }
}
