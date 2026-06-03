using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuEventSystemHandler : MonoBehaviour
{
    [Header("References")]
    public List<Selectable> Selectables = new();

    [Header("Animations")]
    [SerializeField] protected float _selectedAnimationScale = 1.1f;
    [SerializeField] protected float _scaleDuration = 1.0f;

    protected Dictionary<Selectable, Vector3> _scales = new Dictionary<Selectable, Vector3>();
    
    protected virtual void AddSelectionListeners(Selectable selectable)
    {
        // Get event trigger
        EventTrigger trigger = selectable.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = selectable.gameObject.AddComponent<EventTrigger>();
        }

        // SELECT event
        EventTrigger.Entry selectEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.Select
        };
        selectEntry.callback.AddListener(OnSelect);
        trigger.triggers.Add(selectEntry);

        // DESELECT event
        EventTrigger.Entry deselectEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.Deselect
        };
        deselectEntry.callback.AddListener(OnDeselect);
        trigger.triggers.Add(deselectEntry);
    }

    public void OnSelect(BaseEventData eventData)
    {

    }
    public void OnDeselect(BaseEventData eventData)
    {

    }
}
