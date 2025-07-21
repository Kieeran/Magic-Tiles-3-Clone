using System.Collections;
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

    float _bpm = 205f;
    public float StepDuration { get; private set; }
    public float StepCountToLine { get; private set; }
    public float DistanceToLine { get; private set; }

    Dictionary<Tile_SO, float> _spawnTimes;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        StepDuration = 60f / _bpm;
        _spawnAllTiles = false;
    }

    void Start()
    {
        Init();

        _spawnTimes = new Dictionary<Tile_SO, float>();
        foreach (Tile_SO tile in CurrentLevel.Tiles)
        {
            float hitTime = tile.StepIndex * StepDuration;
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
        BaseSpawnY = Screen.height + TileSpawner.Instance.StepSpacingY;
        HitLineY = UIManager.Instance.MainCanvas.VerticleLine.anchoredPosition.y;

        DistanceToLine = math.abs(BaseSpawnY) + math.abs(HitLineY);
        StepCountToLine = DistanceToLine / TileSpawner.Instance.StepSpacingY;

        FallTime = StepCountToLine * StepDuration;
        FallSpeed = DistanceToLine / FallTime;
    }

    void UpdateTileSpawn(float currentTime, Dictionary<Tile_SO, float> spawnTimes)
    {
        Debug.Log(currentTime);
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
