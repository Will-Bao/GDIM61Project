using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HideableObject : MonoBehaviour
{
    [Header("Collider")]
    [SerializeField] private GameObject _collider;
    [SerializeField] private LevelMarker _levelMarker;

    private bool _isPlayerInside;
    private Player _player;

    private void Start()
    {
        ToggleCollision(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponentInParent<Player>();
        if (collision.CompareTag("Player") && player != null &&
            _levelMarker.CurrentLayer == LevelManager.Instance.CurrentLevel)
        {
            _isPlayerInside = true;
            _player = player;

            player.ToggleHiding(player.IsCrouching);
            LightingManager.Instance.SetTableHidingDarkness(player.IsCrouching);

            player.OnCrouch += HandleCrouch;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.GetComponentInParent<Player>();
        if (collision.CompareTag("Player") && player != null &&
            _levelMarker.CurrentLayer == LevelManager.Instance.CurrentLevel)
        {
            _isPlayerInside = false;
            _player = null;

            player.ToggleHiding(false);
            LightingManager.Instance.SetTableHidingDarkness(false);
            player.OnCrouch -= HandleCrouch;
            //ToggleCollision(false);
        }
    }

    private void HandleCrouch(bool isCrouching)
    {
        if (!_isPlayerInside || !_player) return;

        _player.ToggleHiding(isCrouching);
        LightingManager.Instance.SetTableHidingDarkness(isCrouching);
    }

    private void ToggleCollision(bool active)
    {
        _collider.SetActive(active);
    }
}
