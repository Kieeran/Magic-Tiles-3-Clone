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

    bool _spawnAllTiles;

    // =========================
    float _baseSpawnY = 2750f;
    float _hitLineY = -410f;
    float _fallTime;
    float _fallSpeed;

    float _bpm = 130f;
    float _stepDuration;
    float _stepCountToLine = 3.5f;
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

        spawnTimes = new Dictionary<Tile_SO, float>();
        earlySpawnTimes = new Dictionary<Tile_SO, float>();

        foreach (Tile_SO tile in CurrentLevel.Tiles)
        {
            float hitTime = tile.StepIndex * _stepDuration;
            float spawnTime = hitTime - _fallTime;

            // if (spawnTime < 0)
            // {
            //     earlySpawnTimes.Add(tile, spawnTime);
            //     Debug.Log($"Early spawn time: {spawnTime} at step {tile.StepIndex}");
            // }
            // else
            // {
            //     spawnTimes.Add(tile, spawnTime);
            // }

            spawnTimes.Add(tile, spawnTime);
            Debug.Log($"Spawn time: {spawnTime} at step {tile.StepIndex}");
        }

        // float earliestSpawnTime = earlySpawnTimes.Values.Min();
        float earliestSpawnTime = spawnTimes.Values.Min();
        GameManager.Instance.SetEarliestSpawnTime(earliestSpawnTime);
        Debug.Log(earliestSpawnTime);

        // GameManager.Instance.OnGameStart += () =>
        // {
        //     StartCoroutine(StartEarlyTiles());
        //     Debug.Log("Spawn early");
        // };
    }

    // IEnumerator StartEarlyTiles()
    // {
    //     // float timer = -SoundManager.Instance.AudioStartDelay;

    //     // while (timer < 0)
    //     // {
    //     //     timer += Time.deltaTime;
    //     //     UpdateTileSpawn(timer, earlySpawnTimes);
    //     //     yield return null;
    //     // }

    //     float startTime = Time.realtimeSinceStartup;
    //     float endTime = startTime + SoundManager.Instance.AudioStartDelay;

    //     while (Time.realtimeSinceStartup < endTime)
    //     {
    //         float currentTimer = Time.realtimeSinceStartup - endTime; // timer từ âm đến 0
    //         UpdateTileSpawn(currentTimer, earlySpawnTimes);
    //         yield return null;
    //     }
    // }

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

        // float currentTime = SoundManager.Instance.musicSource.time;
        float currentTime = GameManager.Instance.Timer;
        UpdateTileSpawn(currentTime, spawnTimes);
    }
}
