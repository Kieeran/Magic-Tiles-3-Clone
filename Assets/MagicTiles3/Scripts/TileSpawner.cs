
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    public static TileSpawner Instance { get; private set; }

    [SerializeField] List<Tile> _tilePrefabs;
    public Dictionary<TileType, Tile> TilePrefabs;

    List<Transform> _rows;

    // Tile _currentTile;
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

        // _currentTile = null;
    }

    // void Update()
    // {
    //     if (!@GameManager.Instance.IsGameStart()) return;

    //     _timer += Time.deltaTime;
    //     if (_timer < SpawnCoolDown) return;

    //     _timer = 0;

    //     int randomTileIndex = Random.Range(0, TilePrefabs.Count);

    //     int randomIndexRow = Random.Range(0, _rows.Count);
    //     Tile tile = Instantiate(TilePrefabs[randomTileIndex], _rows[randomIndexRow]);
    //     tile.transform.localPosition = new Vector3(0, _ySpawn, 0);
    //     tile.ResetRectTransform();
    // }

    public void Spawn(Tile_SO tile_SO)
    {
        Tile tile = Instantiate(TilePrefabs[tile_SO.TileType], _rows[tile_SO.RowIndex]);
        tile.SetFallSpeed(LevelManager.Instance.GetFallSpeed());

        tile.GetRectTransform().anchoredPosition = new Vector3(0, _baseSpawnY + 450f, 0);

        tile.ResetRectTransform();
        Debug.Log($"Spawn tile at step {tile_SO.StepIndex}");
    }
}
