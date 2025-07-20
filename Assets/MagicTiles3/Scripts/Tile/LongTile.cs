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
    float _holdTime = 0f;
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
            _holdTime += Time.deltaTime;

            // TouchTransform.sizeDelta = new Vector2(TouchTransform.sizeDelta.x, TouchTransform.sizeDelta.y + 27f);
            TouchTransform.sizeDelta += _fallSpeed * 1.2f * Time.deltaTime * Vector2.up;

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
        _isHolding = true;
        _holdTime = 0f;

        TouchTransform.sizeDelta = originSizeDelta;
        TouchEffect.gameObject.SetActive(true);

        _isTouched = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isHolding = false;
        _holdTime = 0f;

        TouchTransform.sizeDelta = originSizeDelta;
        TouchEffect.gameObject.SetActive(false);
    }
}
