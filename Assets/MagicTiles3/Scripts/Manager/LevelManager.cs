using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public Level_SO CurrentLevel;

    Tile_SO _currentTile_SO;
    float _timer;
    int _currentIndexTile_SO;
    bool _spawnAllTiles;
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
        _spawnAllTiles = false;
        _timer = 0f;
        _currentIndexTile_SO = 0;
        _currentTile_SO = CurrentLevel.Tiles[_currentIndexTile_SO];
    }

    void Update()
    {
        if (!GameManager.Instance.IsGameStart()) return;
        if (_spawnAllTiles) return;

        _timer += Time.deltaTime;
        if (_timer >= _currentTile_SO.AppearTime)
        {
            TileSpawner.Instance.Spawn(_currentTile_SO);

            _currentIndexTile_SO++;

            if (_currentIndexTile_SO < CurrentLevel.Tiles.Count)
            {
                _currentTile_SO = CurrentLevel.Tiles[_currentIndexTile_SO];
            }

            else
            {
                Debug.Log("Spawn all tiles");
                _spawnAllTiles = true;
            }
        }

        MainCanvas.Instance.Timer.text = _timer.ToString();
    }
}
