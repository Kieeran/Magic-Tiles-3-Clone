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
    float _sizeDeltaXLandscapeContainer;

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
    }

    void InitUILayouts()
    {
        InitPortraitLayout();
        InitLandscapeLayout();

        _container = MainCanvas.Container;
    }

    void InitPortraitLayout()
    {
        // Container
        _anchorMinPortraitContainer = new Vector2(0, 0);
        _anchorMaxPortraitContainer = new Vector2(1, 1);
    }

    void InitLandscapeLayout()
    {
        // Container
        _anchorMinLandscapeContainer = new Vector2(0.5f, 0);
        _anchorMaxLandscapeContainer = new Vector2(0.5f, 1);
        _sizeDeltaXLandscapeContainer = 1041f;
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

            OnOrientationPortrait?.Invoke();
        }
        else
        {
            // Landscape
            _container.anchorMin = _anchorMinLandscapeContainer;
            _container.anchorMax = _anchorMaxLandscapeContainer;
            _container.sizeDelta = new Vector2(_sizeDeltaXLandscapeContainer, _container.sizeDelta.y);

            OnOrientationLandscape?.Invoke();
        }
    }
}
