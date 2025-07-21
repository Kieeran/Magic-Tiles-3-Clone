using System;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public TileType TileType;

    protected RectTransform _tileRectTransform;
    protected Image _tileImage;
    protected bool _isTouched;
    protected float _fallSpeed;
    protected float _spawnTime;
    protected float _spawnY;

    public void SetFallSpeed(float speed) { _fallSpeed = speed; }
    public void SetSpawnY(float y) { _spawnY = y; }
    public void SetSpawnTime(float time) { _spawnTime = time; }
    public RectTransform GetRectTransform() { return _tileRectTransform; }

    protected virtual void Awake()
    {
        _tileRectTransform = GetComponent<RectTransform>();
        _tileImage = GetComponent<Image>();

        _isTouched = false;
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
        if (GameManager.Instance.IsGameOver()) return;

        float currentTime = GameManager.Instance.Timer;
        float timeSinceSpawn = currentTime - _spawnTime;

        float newY = _spawnY - timeSinceSpawn * _fallSpeed;
        _tileRectTransform.anchoredPosition = new Vector2(_tileRectTransform.anchoredPosition.x, newY);

        Vector3 tileScreenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, _tileRectTransform.position);

        if (tileScreenPos.y <= UIManager.Instance.MainCanvas.LoseLineScreenPos.y)
        {
            if (!_isTouched)
            {
                Debug.Log("Game Over!");
                // GameManager.Instance.GameOver();
            }
        }

        if (tileScreenPos.y <= UIManager.Instance.MainCanvas.ReturnTileLineScreenPos.y)
        {
            ReturnTile();
        }
    }

    protected virtual void ReturnTile()
    {
        _isTouched = false;

        ResetAlpha();
        TileSpawner.Instance.TilePooling.ReturnTile(this);
    }

    protected void DecreaseAlpha()
    {
        Color color = Color.black;
        color.a = 0.8f;
        _tileImage.color = color;
    }

    protected void ResetAlpha()
    {
        Color color = Color.white;
        _tileImage.color = color;
    }
}
