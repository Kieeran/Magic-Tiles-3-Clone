using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Action OnGameStart;
    public Action OnGameOver;
    public Action OnGameWin;
    bool _isGameStart;
    bool _isGameOver;
    bool _isGameWin;

    float _startTime;
    float _earliestSpawnTime;
    bool _isMusicOn = false;
    public float Timer { get; private set; }

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

    public void SetEarliestSpawnTime(float earliestSpawnTime)
    {
        _earliestSpawnTime = earliestSpawnTime;
        Timer = _earliestSpawnTime;
    }

    void Update()
    {
        if (!_isGameStart) return;

        Timer = Time.realtimeSinceStartup - _startTime + _earliestSpawnTime;

        if (!_isMusicOn && Timer >= -0.55f)
        {
            SoundManager.Instance.PlaySound();
            _isMusicOn = true;
        }
    }

    public void GameStart()
    {
        _isGameStart = true;
        OnGameStart?.Invoke();
        _startTime = Time.realtimeSinceStartup;
    }

    public void GameOver()
    {
        _isGameOver = true;
        Time.timeScale = 0;
        StartCoroutine(GameRestart(3f));
        OnGameOver?.Invoke();
    }

    public void GameWin()
    {
        _isGameWin = true;
        Time.timeScale = 0;
        StartCoroutine(GameRestart(3f));
        OnGameWin?.Invoke();
    }

    public bool IsGameStart()
    {
        return _isGameStart;
    }

    public bool IsGameOver()
    {
        return _isGameOver;
    }

    public bool IsGameWin()
    {
        return _isGameWin;
    }

    IEnumerator GameRestart(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
        Time.timeScale = 1f;

        UIManager.Instance.UnlockOrientation();
    }
}
