using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Action OnGameOver;
    bool _isGameStart;
    bool _isGameOver;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Application.targetFrameRate = 60;
    }

    public void GameOver()
    {
        _isGameOver = true;
        Time.timeScale = 0;
        OnGameOver?.Invoke();
    }

    public void GameStart()
    {
        _isGameStart = true;
    }

    public bool IsGameStart()
    {
        return _isGameStart;
    }

    public bool IsGameOver()
    {
        return _isGameOver;
    }
}
