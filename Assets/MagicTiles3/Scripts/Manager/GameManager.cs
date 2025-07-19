using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        StartCoroutine(GameRestart(2f));
        OnGameOver?.Invoke();
    }

    public void GameStart()
    {
        _isGameStart = true;
        SoundManager.Instance.PlaySoundWithDelay(0.5f);
    }

    public bool IsGameStart()
    {
        return _isGameStart;
    }

    public bool IsGameOver()
    {
        return _isGameOver;
    }

    IEnumerator GameRestart(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
        Time.timeScale = 1f;
    }
}
