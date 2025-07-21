using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public Level_SO CurrentLevel;

    bool _spawnAllTiles;

    // =========================
    public float BaseSpawnY { get; private set; }
    public float HitLineY { get; private set; }
    public float FallTime { get; private set; }
    public float FallSpeed { get; private set; }

    float _bpm = 210f;
    float _stepCountToLine = 3.5f;
    float _stepDuration;
    float _distanceToLine;

    Dictionary<Tile_SO, float> _spawnTimes;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _stepDuration = 60f / _bpm;
        _spawnAllTiles = false;
    }

    void Start()
    {
        Init();

        _spawnTimes = new Dictionary<Tile_SO, float>();
        foreach (Tile_SO tile in CurrentLevel.Tiles)
        {
            float hitTime = tile.StepIndex * _stepDuration;
            float spawnTime = hitTime - FallTime;

            _spawnTimes.Add(tile, spawnTime);
            Debug.Log($"Spawn time: {spawnTime} at step {tile.StepIndex}");
        }

        float earliestSpawnTime = _spawnTimes.Values.Min();
        GameManager.Instance.SetEarliestSpawnTime(earliestSpawnTime);
        Debug.Log(earliestSpawnTime);

        UIManager.Instance.OnOrientationPortrait += () =>
        {
            Init();
        };

        UIManager.Instance.OnOrientationLandscape += () =>
        {
            Init();
        };
    }

    void Init()
    {
        HitLineY = UIManager.Instance.MainCanvas.VerticleLineY;
        FallTime = _stepCountToLine * _stepDuration;

        if (UIManager.Instance.IsPortrait())
        {
            BaseSpawnY = Screen.height * 1.5f;
        }
        else
        {
            BaseSpawnY = Screen.height;
        }

        _distanceToLine = BaseSpawnY + math.abs(HitLineY);
        TileSpawner.Instance.StepSpacingY = _distanceToLine / _stepCountToLine;
        FallSpeed = _distanceToLine / FallTime;

        // Debug.Log(_distanceToLine);
        // Debug.Log(HitLineY);
        // Debug.Log(TileSpawner.Instance.StepSpacingY);
    }

    public bool HandleNonTileTouch(Vector2 screenPosition, int rowIndex)
    {
        Tile closestTile = null;
        float closestDistance = float.MaxValue;
        for (int i = 0; i < TileSpawner.Instance.Rows.Count; i++)
        {
            foreach (Transform child in TileSpawner.Instance.Rows[i])
            {
                if (child.gameObject.CompareTag("Tile"))
                {
                    Tile tile = child.GetComponent<Tile>();
                    Vector2 tileScreenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, tile.TileRectTransform.position);

                    float distance = Vector2.Distance(tileScreenPos, screenPosition);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestTile = tile;
                    }
                }
            }
        }

        if (closestTile != null)
        {
            if (!IsScreenYInsideTileYBounds(closestTile, screenPosition)) return false;

            Debug.Log($"Closest tile: {closestTile.name} at distance: {closestDistance} at row {rowIndex}");
            Debug.Log(closestTile.TileRectTransform.anchoredPosition);

            TileSpawner.Instance.SpawnMissTile(
                rowIndex,
                closestTile.TileRectTransform.anchoredPosition.y,
                closestTile.TileRectTransform.sizeDelta.y
            );
        }

        return true;
    }

    bool IsScreenYInsideTileYBounds(Tile tile, Vector2 screenPos)
    {
        Vector3[] corners = new Vector3[4];
        tile.TileRectTransform.GetWorldCorners(corners);

        Vector2 bottom = RectTransformUtility.WorldToScreenPoint(Camera.main, corners[0]);
        Vector2 top = RectTransformUtility.WorldToScreenPoint(Camera.main, corners[1]);

        float minY = Mathf.Min(bottom.y, top.y);
        float maxY = Mathf.Max(bottom.y, top.y);

        return screenPos.y >= minY && screenPos.y <= maxY;
    }

    void UpdateTileSpawn(float currentTime, Dictionary<Tile_SO, float> spawnTimes)
    {
        // Debug.Log(currentTime);
        List<Tile_SO> tilesToRemove = new();
        foreach (var entry in spawnTimes)
        {
            if (currentTime >= entry.Value)
            {
                // Debug.Log(entry.Value);
                TileSpawner.Instance.Spawn(entry.Key, entry.Value);

                tilesToRemove.Add(entry.Key);
            }
        }

        foreach (var key in tilesToRemove)
        {
            spawnTimes.Remove(key);
        }
    }

    void Update()
    {
        if (!GameManager.Instance.IsGameStart()) return;
        if (_spawnAllTiles) return;

        float currentTime = GameManager.Instance.Timer;
        UpdateTileSpawn(currentTime, _spawnTimes);
    }
}
