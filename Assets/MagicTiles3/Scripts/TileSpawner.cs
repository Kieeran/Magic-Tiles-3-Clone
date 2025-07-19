
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    public static TileSpawner Instance { get; private set; }

    [SerializeField] List<Tile> _tilePrefabs;
    public Dictionary<TileType, Tile> TilePrefabs;

    List<Transform> _rows;

    float _stepSpacingY = 900f;
    float _baseSpawnY = 1745f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        TilePrefabs = new Dictionary<TileType, Tile>();
        foreach (Tile tile in _tilePrefabs)
        {
            if (tile.GetType() == typeof(ShortTile))
            {
                TilePrefabs.Add(TileType.Short, tile);
            }

            else if (tile.GetType() == typeof(LongTile))
            {
                TilePrefabs.Add(TileType.Long, tile);
            }
        }

        _rows = new List<Transform>();
        foreach (Transform child in transform)
        {
            _rows.Add(child);
        }
    }

    public void Spawn(Tile_SO tile_SO)
    {
        Tile tile = Instantiate(TilePrefabs[tile_SO.TileType], _rows[tile_SO.RowIndex]);
        tile.SetFallSpeed(LevelManager.Instance.GetFallSpeed());

        tile.GetRectTransform().anchoredPosition = new Vector3(0, _baseSpawnY + tile.GetRectTransform().sizeDelta.y / 2, 0);

        if (tile_SO.StepLength > 1)
        {
            float height = tile_SO.StepLength * _stepSpacingY;
            tile.GetRectTransform().sizeDelta = new Vector2(tile.GetRectTransform().sizeDelta.x, height);
        }

        tile.ResetRectTransform();
        Debug.Log($"Spawn tile at step {tile_SO.StepIndex}");
    }
}
