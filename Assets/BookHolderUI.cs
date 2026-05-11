using UnityEngine;

public class BookHolderUI : MonoBehaviour
{
    [SerializeField] private PlayerBookHandler _playerBookHandler;
    [SerializeField] private GameObject _bookIndicator;

    private void Update()
    {
        if (_playerBookHandler == null || _bookIndicator == null) return;

        _bookIndicator.SetActive(_playerBookHandler.HasBook);
    }
}
