using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Encounter/Scripted Encounter")]
public class Encounter : ScriptableObject
{
    [Header("Encounter Events")]
    [SerializeField] private List<EncounterStep> _steps;

    public IEnumerator Run(EncounterContext context)
    {
        foreach (var step in _steps)
        {
            yield return step.Execute(context);
        }
    }
}
