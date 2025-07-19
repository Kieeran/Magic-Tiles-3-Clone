using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    protected RectTransform _tileRectTransform;
    protected Image _tileImage;

    protected virtual void Awake()
    {
        _tileRectTransform = GetComponent<RectTransform>();
        _tileImage = GetComponent<Image>();
    }

    protected virtual void Start()
    {

    }

    public void ResetRectTransform()
    {
        _tileRectTransform.offsetMin = new Vector2(0, _tileRectTransform.offsetMin.y);
        _tileRectTransform.offsetMax = new Vector2(0, _tileRectTransform.offsetMax.y);
    }

    protected virtual void Update()
    {
        transform.position += new Vector3(0, -0.1f, 0);

        if (transform.localPosition.y < -3000f)
        {
            Destroy(gameObject);
        }
    }

    protected void DecreaseAlpha()
    {
        Color color = Color.black;
        color.a = 0.8f;
        _tileImage.color = color;
        Debug.Log("Aloo");
    }

    protected void ResetAlpha()
    {
        Color color = Color.black;
        _tileImage.color = color;
        Debug.Log("Olaa");
    }
}
