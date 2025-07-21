
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    public static TileSpawner Instance { get; private set; }
    public float StepSpacingY;
    public TilePooling TilePooling;

    [SerializeField] Transform _rowsContainer;
    public List<Transform> Rows { get; private set; }

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
        Rows = new List<Transform>();
        foreach (Transform child in _rowsContainer)
        {
            Rows.Add(child);
        }
    }

    public void Spawn(Tile_SO tile_SO, float spawnTime)
    {
        Tile tile = TilePooling.GetTileByType(tile_SO.TileType);
        tile.transform.SetParent(Rows[tile_SO.RowIndex]);
        tile.SetFallSpeed(LevelManager.Instance.FallSpeed);
        tile.SetSpawnTime(spawnTime);
        RectTransform rect = tile.GetRectTransform();

        float height = tile_SO.StepLength * StepSpacingY;
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, height);

        tile.ResetRectTransform();
        rect.anchoredPosition = new Vector3(0, LevelManager.Instance.BaseSpawnY + rect.sizeDelta.y / 2, 0);
        tile.SetSpawnY(LevelManager.Instance.BaseSpawnY + rect.sizeDelta.y / 2);
        Debug.Log($"Spawn tile at step {tile_SO.StepIndex}, pos: {rect.anchoredPosition}");
    }

    public void SpawnMissTile(int rowIndex, float spawnY, float height)
    {
        RectTransform missTile = Instantiate(TilePooling.MissTile, Rows[rowIndex]);
        missTile.anchoredPosition = new Vector2(missTile.anchoredPosition.x, spawnY);
        missTile.sizeDelta = new Vector2(missTile.sizeDelta.x, height);
    }

    public bool HasAnyTile()
    {
        foreach (Transform row in Rows)
        {
            foreach (Transform child in row)
            {
                if (child.CompareTag("Tile"))
                {
                    return true;
                }
            }
        }
        return false;
    }
}