using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HideableObject : MonoBehaviour
{
    [Header("Collider")]
    [SerializeField] private GameObject _collider;
    void Start()
    {
        
    }

    private void ToggleCollision(bool active)
    {
        _collider.SetActive(active);
    }
}
