using UnityEngine;
using UnityEngine.UI;

public class StartTile : MonoBehaviour
{
    RectTransform _rect;

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            GameManager.Instance.GameStart();

            Destroy(gameObject);
        });
    }

    void Start()
    {
        _rect = GetComponent<RectTransform>();
        UIManager.Instance.OnOrientationPortrait += () =>
        {
            _rect.sizeDelta = new Vector2(_rect.sizeDelta.x, TileSpawner.Instance.StepSpacingY);
            _rect.anchoredPosition = new Vector2(_rect.anchoredPosition.x, UIManager.Instance.MainCanvas.VerticleLine.anchoredPosition.y);
        };

        UIManager.Instance.OnOrientationLandscape += () =>
        {
            _rect.sizeDelta = new Vector2(_rect.sizeDelta.x, TileSpawner.Instance.StepSpacingY);
            _rect.anchoredPosition = new Vector2(_rect.anchoredPosition.x, UIManager.Instance.MainCanvas.VerticleLine.anchoredPosition.y);
        };
    }
}
