using UnityEngine;

public class MainCanvas : MonoBehaviour
{
    public Transform GameOverPopUp;
    public RectTransform Container;
    public RectTransform VerticleLine;
    [SerializeField] RectTransform _loseLine;
    [SerializeField] RectTransform _returnTileLine;

    public Vector3 LoseLineScreenPos { get; private set; }
    public Vector3 ReturnTileLineScreenPos { get; private set; }

    float _verticleLineYPortrait;
    float _verticleLineYLandscape;
    public float VerticleLineY { get; private set; }

    void Awake()
    {
        if (Screen.height > Screen.width)
        {
            _verticleLineYPortrait = -Screen.height * 0.2f;
            _verticleLineYLandscape = -Screen.width * 0.2f;

            VerticleLineY = _verticleLineYPortrait;
            VerticleLine.anchoredPosition = new Vector2(VerticleLine.anchoredPosition.x, VerticleLineY);
        }

        else
        {
            _verticleLineYPortrait = -Screen.width * 0.2f;
            _verticleLineYLandscape = -Screen.height * 0.2f;

            VerticleLineY = _verticleLineYLandscape;
            VerticleLine.anchoredPosition = new Vector2(VerticleLine.anchoredPosition.x, VerticleLineY);
        }

        LoseLineScreenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, _loseLine.position);
        ReturnTileLineScreenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, _returnTileLine.position);
    }

    void Start()
    {
        GameManager.Instance.OnGameOver += () =>
        {
            GameOverPopUp.gameObject.SetActive(true);
        };

        UIManager.Instance.OnOrientationPortrait += () =>
        {
            VerticleLineY = _verticleLineYPortrait;
            VerticleLine.anchoredPosition = new Vector2(VerticleLine.anchoredPosition.x, VerticleLineY);
        };

        UIManager.Instance.OnOrientationLandscape += () =>
        {
            VerticleLineY = _verticleLineYLandscape;
            VerticleLine.anchoredPosition = new Vector2(VerticleLine.anchoredPosition.x, VerticleLineY);
        };
    }
}
