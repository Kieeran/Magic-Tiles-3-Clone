using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tile : MonoBehaviour, IPointerDownHandler
{
    RectTransform _tileRectTransform;
    Image _tileImage;

    void Awake()
    {
        _tileRectTransform = GetComponent<RectTransform>();
        _tileImage = GetComponent<Image>();
    }

    public void ResetRectTransform()
    {
        _tileRectTransform.offsetMin = new Vector2(0, _tileRectTransform.offsetMin.y);
        _tileRectTransform.offsetMax = new Vector2(0, _tileRectTransform.offsetMax.y);
    }

    void Update()
    {
        transform.position += new Vector3(0, -0.1f, 0);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Color color = Color.black;
        color.a = 0.8f;
        _tileImage.color = color;
    }
}
