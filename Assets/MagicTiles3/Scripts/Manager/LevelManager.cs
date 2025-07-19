using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public Level_SO CurrentLevel;
    public float SpawnCoolDown = 0.5f;

    Tile_SO _currentTile_SO;
    float _timer;
    int _currentIndexTile_SO;
    bool _spawnAllTiles;

    // =========================
    float _baseSpawnY = 1745f;
    float _hitLineY = -410f;
    float _fallTime;
    float _fallSpeed;

    float _bpm = 130f;
    float _stepDuration;
    // float _stepSpacingY = 900f;
    float _stepCountToLine = 2.5f;
    float _distanceToLine;

    Dictionary<Tile_SO, float> spawnTimes;

    public float GetFallSpeed() { return _fallSpeed; }

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
        _distanceToLine = math.abs(_baseSpawnY) + math.abs(_hitLineY);
        _stepDuration = 60f / _bpm;
        _fallTime = _stepCountToLine * _stepDuration;
        _fallSpeed = _distanceToLine / _fallTime;

        _spawnAllTiles = false;
        _timer = SpawnCoolDown;
        _currentIndexTile_SO = 0;
        _currentTile_SO = CurrentLevel.Tiles[_currentIndexTile_SO];

        spawnTimes = new Dictionary<Tile_SO, float>();

        foreach (Tile_SO tile in CurrentLevel.Tiles)
        {
            float hitTime = tile.StepIndex * _stepDuration;
            float spawnTime = Mathf.Max(hitTime - _fallTime, 0);

            spawnTimes.Add(tile, spawnTime);
        }
    }

    void Update()
    {
        if (!GameManager.Instance.IsGameStart()) return;
        if (_spawnAllTiles) return;

        float currentTime = SoundManager.Instance.musicSource.time;

        foreach (var entry in spawnTimes)
        {
            if (currentTime >= entry.Value)
            {
                TileSpawner.Instance.Spawn(entry.Key);
                spawnTimes.Remove(entry.Key);
            }
        }
    }
}
