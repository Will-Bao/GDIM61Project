using System.Collections;
using UnityEngine;

public abstract class EncounterStep : ScriptableObject
{
    public abstract IEnumerator Execute(EncounterContext context);
}

public class EncounterContext
{
    public Transform Player;
    public int Layer;
    public Vector3 Location;
    // TODO: add more setting values for encounter here
}
