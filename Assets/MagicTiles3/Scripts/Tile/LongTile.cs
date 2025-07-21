using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LongTile : Tile, IPointerDownHandler, IPointerUpHandler
{
    public float holdThreshold = 0.1f;
    public Image TouchEffect;
    public RectTransform TouchTransform;

    float tileHeight;
    bool _isHolding = false;
    Vector2 originSizeDelta;

    protected override void Start()
    {
        originSizeDelta = TouchTransform.sizeDelta;
        tileHeight = _tileRectTransform.sizeDelta.y;
    }

    protected override void Update()
    {
        base.Update();

        if (_isHolding)
        {
            TouchTransform.sizeDelta += _fallSpeed * Time.deltaTime * Vector2.up;

            if (TouchTransform.sizeDelta.y >= tileHeight)
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

        // TouchTransform.sizeDelta = originSizeDelta;
        // TouchEffect.gameObject.SetActive(false);
    }
}
