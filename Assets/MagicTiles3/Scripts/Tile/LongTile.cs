using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LongTile : Tile, IPointerDownHandler, IPointerUpHandler
{
    public float holdThreshold = 0.1f;
    public Image TouchEffect;
    public RectTransform TouchTransform;

    bool _isHolding = false;
    Vector2 originSizeDelta;

    protected override void Start()
    {
        originSizeDelta = TouchTransform.sizeDelta;

        UIManager.Instance.OnOrientationPortrait += () =>
        {
            originSizeDelta = TouchTransform.sizeDelta;
        };

        UIManager.Instance.OnOrientationLandscape += () =>
        {
            originSizeDelta = TouchTransform.sizeDelta;
        };
    }

    protected override void Update()
    {
        base.Update();

        if (_isHolding)
        {
            TouchTransform.sizeDelta += _fallSpeed * Time.deltaTime * Vector2.up;

            if (TouchTransform.rect.height > TileRectTransform.rect.height)
            {
                TouchEffect.gameObject.SetActive(false);
                _isHolding = false;
                DecreaseAlpha();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_isTouched) return;

        _isHolding = true;
        TouchTransform.sizeDelta = originSizeDelta;
        TouchEffect.gameObject.SetActive(true);
        _isTouched = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isHolding = false;
    }

    protected override void ReturnTile()
    {
        base.ReturnTile();

        TouchTransform.sizeDelta = originSizeDelta;
        TouchEffect.gameObject.SetActive(false);
    }
}
