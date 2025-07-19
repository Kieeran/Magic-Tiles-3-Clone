using UnityEngine;

public class Tile : MonoBehaviour
{
    RectTransform tileRectTransform;

    void Awake()
    {
        tileRectTransform = GetComponent<RectTransform>();
    }

    public void ResetRectTransform()
    {
        tileRectTransform.offsetMin = new Vector2(0, tileRectTransform.offsetMin.y);
        tileRectTransform.offsetMax = new Vector2(0, tileRectTransform.offsetMax.y);
    }

    void Update()
    {
        transform.position += new Vector3(0, -0.1f, 0);
    }
}
