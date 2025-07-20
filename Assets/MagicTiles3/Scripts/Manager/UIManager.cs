using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    bool _isPortrait;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
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
            
        }
        else
        {
            // Landscape

        }
    }
}
