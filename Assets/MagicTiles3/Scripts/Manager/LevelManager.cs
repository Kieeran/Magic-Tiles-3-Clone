using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    float _stepCountToLine = 2.5f;
    float _distanceToLine;

    Dictionary<Tile_SO, float> spawnTimes;
    Dictionary<Tile_SO, float> earlySpawnTimes;

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
        earlySpawnTimes = new Dictionary<Tile_SO, float>();

        foreach (Tile_SO tile in CurrentLevel.Tiles)
        {
            float hitTime = tile.StepIndex * _stepDuration;
            float spawnTime = hitTime - _fallTime;

            if (spawnTime < 0)
            {
                earlySpawnTimes.Add(tile, -spawnTime);
                Debug.Log($"Early spawn time: {spawnTime} at step {tile.StepIndex}");
            }
            else
            {
                spawnTimes.Add(tile, spawnTime);
            }
        }

        // Get the earliest spawn time (most negative value originally).
        // Since all times are negative, using Max() after removing the minus sign
        // effectively gives us the largest negative (i.e., the earliest) time.
        float earliestSpawnTime = earlySpawnTimes.Values.Max();

        SoundManager.Instance.AudioStartDelay = earliestSpawnTime - 0.18f;
        Debug.Log(earliestSpawnTime);

        GameManager.Instance.OnGameStart += () =>
        {
            StartCoroutine(StartEarlyTiles());
            Debug.Log("Spawn early");
        };
    }

    IEnumerator StartEarlyTiles()
    {
        float timer = 0f;

        while (timer < SoundManager.Instance.AudioStartDelay)
        {
            timer += Time.deltaTime;
            UpdateTileSpawn(timer, earlySpawnTimes);
            yield return null;
        }
    }

    void UpdateTileSpawn(float currentTime, Dictionary<Tile_SO, float> spawnTimes)
    {
        List<Tile_SO> tilesToRemove = new();
        foreach (var entry in spawnTimes)
        {
            if (currentTime >= entry.Value)
            {
                // Debug.Log(entry.Value);
                TileSpawner.Instance.Spawn(entry.Key);

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

        float currentTime = SoundManager.Instance.musicSource.time;
        UpdateTileSpawn(currentTime, spawnTimes);
    }
}
