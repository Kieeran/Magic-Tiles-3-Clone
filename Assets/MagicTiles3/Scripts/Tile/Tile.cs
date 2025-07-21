using System;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public TileType TileType;

    public RectTransform TileRectTransform { get; protected set; }
    protected Image _tileImage;
    protected bool _isTouched;
    protected float _fallSpeed;
    protected float _spawnTime;
    protected float _spawnY;

    public void SetFallSpeed(float speed) { _fallSpeed = speed; }
    public void SetSpawnY(float y) { _spawnY = y; }
    public void SetSpawnTime(float time) { _spawnTime = time; }
    public RectTransform GetRectTransform() { return TileRectTransform; }

    protected virtual void Awake()
    {
        TileRectTransform = GetComponent<RectTransform>();
        _tileImage = GetComponent<Image>();

        _isTouched = false;
    }

    protected virtual void Start()
    {

    }

    public void ResetRectTransform()
    {
        TileRectTransform.offsetMin = new Vector2(0, TileRectTransform.offsetMin.y);
        TileRectTransform.offsetMax = new Vector2(0, TileRectTransform.offsetMax.y);
    }

    protected virtual void Update()
    {
        if (GameManager.Instance.IsGameOver()) return;

        float currentTime = GameManager.Instance.Timer;
        float timeSinceSpawn = currentTime - _spawnTime;

        float newY = _spawnY - timeSinceSpawn * _fallSpeed;
        TileRectTransform.anchoredPosition = new Vector2(TileRectTransform.anchoredPosition.x, newY);

        Vector3 tileScreenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, TileRectTransform.position);

        if (tileScreenPos.y <= UIManager.Instance.MainCanvas.LoseLineScreenPos.y)
        {
            if (!_isTouched)
            {
                Debug.Log("Game Over!");
                GameManager.Instance.GameOver();
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
