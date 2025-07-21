using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public MainCanvas MainCanvas;
    public event Action OnOrientationPortrait;
    public event Action OnOrientationLandscape;

    bool _isPortrait;

    //============================Container============================
    RectTransform _container;
    // Portrait
    Vector2 _anchorMinPortraitContainer;
    Vector2 _anchorMaxPortraitContainer;

    // Landscape
    Vector2 _anchorMinLandscapeContainer;
    Vector2 _anchorMaxLandscapeContainer;

    //============================LeftContainer============================
    RectTransform _leftContainer;
    // Portrait
    Vector2 _sizeDataPortraitLeftContainer;

    // Landscape
    Vector2 _sizeDataLandscapeLeftContainer;

    //============================RightContainer============================
    RectTransform _rightContainer;
    // Portrait
    Vector2 _sizeDataPortraitRightContainer;

    // Landscape
    Vector2 _sizeDataLandscapeRightContainer;

    //============================ProgressBar============================
    RectTransform _progressBar;
    // Portrait
    Vector2 _sizeDataPortraitProgressBar;

    // Landscape
    Vector2 _sizeDataLandscapeProgressBar;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        InitUILayouts();
        UpdateLayout();

        GameManager.Instance.OnGameStart += () =>
        {
            LockCurrentOrientation();
        };
    }

    void LockCurrentOrientation()
    {
        switch (Screen.orientation)
        {
            case ScreenOrientation.Portrait:
                Screen.orientation = ScreenOrientation.Portrait;
                break;
            case ScreenOrientation.LandscapeLeft:
                Screen.orientation = ScreenOrientation.LandscapeLeft;
                break;
            case ScreenOrientation.LandscapeRight:
                Screen.orientation = ScreenOrientation.LandscapeRight;
                break;
            case ScreenOrientation.PortraitUpsideDown:
                Screen.orientation = ScreenOrientation.PortraitUpsideDown;
                break;
            default:
                Screen.orientation = ScreenOrientation.Portrait; // fallback
                break;
        }

        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
    }

    public void UnlockOrientation()
    {
        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = true;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
    }

    public bool IsPortrait()
    {
        return Screen.height > Screen.width;
    }

    void InitUILayouts()
    {
        InitPortraitLayout();
        InitLandscapeLayout();

        _container = MainCanvas.Container;
        _leftContainer = MainCanvas.LeftContainer;
        _rightContainer = MainCanvas.RightContainer;
        _progressBar = MainCanvas.ProgressBar;
    }

    void InitPortraitLayout()
    {
        // Container
        _anchorMinPortraitContainer = new Vector2(0, 0);
        _anchorMaxPortraitContainer = new Vector2(1, 1);

        // LeftContainer
        _sizeDataPortraitLeftContainer = new Vector2(850f, 255f);

        // RightContainer
        _sizeDataPortraitRightContainer = new Vector2(490f, 275f);

        // ProgressBar
        _sizeDataPortraitProgressBar = new Vector2(1600f, 146f);
    }

    void InitLandscapeLayout()
    {
        // Container
        _anchorMinLandscapeContainer = new Vector2(0.5f, 0);
        _anchorMaxLandscapeContainer = new Vector2(0.5f, 1);

        // LeftContainer
        _sizeDataLandscapeLeftContainer = new Vector2(465f, 185f);

        // RightContainer
        _sizeDataLandscapeRightContainer = new Vector2(240f, 175f);

        // ProgressBar
        _sizeDataLandscapeProgressBar = new Vector2(610f, 52f);
    }

    void Update()
    {
        if (IsOrientationChanged())
        {
            UpdateLayout();
        }
    }

    bool IsOrientationChanged()
    {
        bool nowPortrait = Screen.height > Screen.width;
        if (nowPortrait != _isPortrait)
        {
            _isPortrait = nowPortrait;
            return true;
        }
        return false;
    }

    void UpdateLayout()
    {
        if (Screen.height > Screen.width)
        {
            // Portrait
            _container.anchorMin = _anchorMinPortraitContainer;
            _container.anchorMax = _anchorMaxPortraitContainer;

            _container.offsetMin = Vector2.zero;
            _container.offsetMax = Vector2.zero;

            _leftContainer.sizeDelta = _sizeDataPortraitLeftContainer;
            _rightContainer.sizeDelta = _sizeDataPortraitRightContainer;

            _progressBar.sizeDelta = _sizeDataPortraitProgressBar;

            OnOrientationPortrait?.Invoke();
        }
        else
        {
            // Landscape
            _container.anchorMin = _anchorMinLandscapeContainer;
            _container.anchorMax = _anchorMaxLandscapeContainer;
            _container.sizeDelta = new Vector2(Screen.width * 0.38f, _container.sizeDelta.y);

            _leftContainer.sizeDelta = _sizeDataLandscapeLeftContainer;
            _rightContainer.sizeDelta = _sizeDataLandscapeRightContainer;

            _progressBar.sizeDelta = _sizeDataLandscapeProgressBar;

            OnOrientationLandscape?.Invoke();
        }
    }
}
