using UnityEngine;

public class MainCanvas : MonoBehaviour
{
    public Transform GameOverPopUp;
    public RectTransform Container;
    public RectTransform VerticleLine;

    float _verticleLineYPortrait = -Screen.height * 0.2f;
    float _verticleLineYLandscape = -Screen.width * 0.2f;

    void Start()
    {
        GameManager.Instance.OnGameOver += () =>
        {
            GameOverPopUp.gameObject.SetActive(true);
        };

        UIManager.Instance.OnOrientationPortrait += () =>
        {
            VerticleLine.anchoredPosition = new Vector2(VerticleLine.anchoredPosition.x, _verticleLineYPortrait);
        };

        UIManager.Instance.OnOrientationLandscape += () =>
        {
            VerticleLine.anchoredPosition = new Vector2(VerticleLine.anchoredPosition.x, _verticleLineYLandscape);
        };
    }
}
