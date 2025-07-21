
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    public static TileSpawner Instance { get; private set; }

    [SerializeField] List<Tile> _tilePrefabs;
    public Dictionary<TileType, Tile> TilePrefabs;
    public float StepSpacingY;
    List<Transform> _rows;

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

    public void Spawn(Tile_SO tile_SO, float spawnTime)
    {
        Tile tile = Instantiate(TilePrefabs[tile_SO.TileType], _rows[tile_SO.RowIndex]);
        tile.SetFallSpeed(LevelManager.Instance.FallSpeed);
        tile.SetSpawnTime(spawnTime);
        RectTransform rect = tile.GetRectTransform();

        // if (tile_SO.StepLength > 1 && tile_SO.TileType == TileType.Long)
        // {
        //     float height = tile_SO.StepLength * StepSpacingY;
        //     rect.sizeDelta = new Vector2(rect.sizeDelta.x, height);
        // }

        float height = tile_SO.StepLength * StepSpacingY;
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, height);

        tile.ResetRectTransform();
        rect.anchoredPosition = new Vector3(0, LevelManager.Instance.BaseSpawnY + rect.sizeDelta.y / 2, 0);
        tile.SetSpawnY(LevelManager.Instance.BaseSpawnY + rect.sizeDelta.y / 2);
        Debug.Log($"Spawn tile at step {tile_SO.StepIndex}, pos: {rect.anchoredPosition}");
    }
}
