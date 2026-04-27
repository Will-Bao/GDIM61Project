using UnityEngine;

[RequireComponent(typeof(LevelMarker))]
public class EncounterTrigger : MonoBehaviour
{
    public Encounter encounter;
    private bool triggered = false;

    private LevelMarker _marker;

    private void Start()
    {
        _marker = GetComponent<LevelMarker>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") ||
            LevelManager.Instance.CurrentLevel != _marker.CurrentLayer ||
            triggered) return;

        triggered = true;

        EncounterContext context = new EncounterContext
        {
            Player = other.transform,
            Layer = _marker.CurrentLayer,
            Location = other.transform.position
        };

        StartCoroutine(encounter.Run(context));
    }
}
