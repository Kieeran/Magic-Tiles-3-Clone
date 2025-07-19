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
        _isTouched = true;
        DecreaseAlpha();
    }
}
