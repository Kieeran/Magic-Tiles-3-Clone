using UnityEngine;
using UnityEngine.EventSystems;

public class ShortTile : Tile, IPointerDownHandler
{
    protected override void Update()
    {
        base.Update();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager.Instance.IsGameOver()) return;
        _isTouched = true;
        DecreaseAlpha();
    }
}
