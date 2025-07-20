using UnityEngine;

public class MainCanvas : MonoBehaviour
{
    public Transform GameOverPopUp;

    void Start()
    {
        GameManager.Instance.OnGameOver += () =>
        {
            GameOverPopUp.gameObject.SetActive(true);
        };
    }
}
