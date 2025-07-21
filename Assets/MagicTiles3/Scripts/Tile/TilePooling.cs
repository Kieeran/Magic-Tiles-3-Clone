using System.Collections.Generic;
using UnityEngine;

public class TilePooling : MonoBehaviour
{
    [SerializeField] List<Tile> _tilePrefabs;
    public Dictionary<TileType, Tile> TilePrefabs { get; private set; }

    Dictionary<TileType, List<Tile>> _tilePools;
    [SerializeField] RectTransform _poolContainer;
    int _poolSize = 5;

    void Awake()
    {
        InitPooling();
    }

    void InitPooling()
    {
        TilePrefabs = new Dictionary<TileType, Tile>();
        _tilePools = new Dictionary<TileType, List<Tile>>();

        foreach (Tile tile in _tilePrefabs)
        {
            TilePrefabs.Add(
                tile.TileType,
                tile
            );

            _tilePools.Add(
                tile.TileType,
                new List<Tile>()
            );
        }

        foreach (TileType type in _tilePools.Keys)
        {
            AddNewTiles(type);
        }
    }

    public void AddNewTiles(TileType tileType)
    {
        for (int i = 0; i < _poolSize; i++)
        {
            Tile tile = Instantiate(TilePrefabs[tileType], _poolContainer);
            tile.gameObject.SetActive(false);
            _tilePools[tileType].Add(tile);
        }
    }

    public Tile GetTileByType(TileType tileType)
    {
        List<Tile> pool = _tilePools[tileType];

        if (pool.Count <= 0)
        {
            AddNewTiles(tileType);
        }

        Tile tile = pool[0];
        pool.RemoveAt(0);
        tile.gameObject.SetActive(true);
        return tile;
    }

    public void ReturnTile(Tile tile)
    {
        TileType type = tile.TileType;
        tile.gameObject.SetActive(false);
        _tilePools[type].Add(tile);
    }
}