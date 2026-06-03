using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanelFade : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private RectTransform _rectTrans;
    [Header("Fade Settings")]
    [SerializeField] private Vector2 _offset = Vector2.zero;
    [SerializeField] private float _fadeDuration = 1.0f;

    private Vector3 _originalPos;

    private void Awake()
    {
        _rectTrans = GetComponent<RectTransform>();
        if ( _rectTrans == null )
        {
            Debug.LogWarning($"Fade transition missing rect transform on {name}");
        }
        _canvasGroup = GetComponent<CanvasGroup>();
        _originalPos = _rectTrans.anchoredPosition;
        Debug.Log($"Original canvas pos {_originalPos}");
    }

    private void OnEnable()
    {
        FadeIn();
    }

    public void FadeIn()
    {
        if (_canvasGroup == null || _rectTrans == null) return;

            _canvasGroup.alpha = 0;
        _rectTrans.anchoredPosition = new Vector3(_offset.x, _offset.y, 0) + _originalPos;
        Debug.Log($"Offset canvas pos {_rectTrans.transform.localPosition}");
        _rectTrans.DOAnchorPos((Vector2)_originalPos, _fadeDuration);
        _canvasGroup.DOFade(1, _fadeDuration);
    }

    public void FadeOut()
    {
        if (_canvasGroup == null ||  _rectTrans == null)
        {
            gameObject.SetActive(false);
            return;
        }

        _canvasGroup.alpha = 1;
        _rectTrans.anchoredPosition = _originalPos;
        _rectTrans.DOAnchorPos(_offset + (Vector2)_originalPos, _fadeDuration);
        _canvasGroup.DOFade(0, _fadeDuration).OnComplete(() => gameObject.SetActive(false));
    }
}
