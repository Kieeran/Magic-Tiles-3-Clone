using TMPro;
using UnityEngine;

public class MainCanvas : MonoBehaviour
{
    public static MainCanvas Instance { get; private set; }

    public Transform GameOverPopUp;
    public TMP_Text Timer;

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
        GameManager.Instance.OnGameOver += () =>
        {
            GameOverPopUp.gameObject.SetActive(true);
        };
    }
}
