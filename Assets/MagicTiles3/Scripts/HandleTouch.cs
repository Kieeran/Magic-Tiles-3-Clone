using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandleTouch : MonoBehaviour
{
    void Update()
    {
        if (!GameManager.Instance.IsGameStart()) return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                IsPointerOverUI(touch.position);
            }
        }

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Input.mousePosition;
            IsPointerOverUI(mousePosition);
        }
#endif
    }

    void IsPointerOverUI(Vector2 position)
    {
        PointerEventData eventData = new(EventSystem.current)
        {
            position = position
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        int rowIndex = -1;
        if (results.Count > 0)
        {
            foreach (var ui in results)
            {
                if (ui.gameObject.CompareTag("Tile"))
                {
                    // Debug.Log("Tile");
                    return;
                }

                if (ui.gameObject.CompareTag("Row"))
                {
                    rowIndex = ui.gameObject.transform.GetSiblingIndex();
                }
            }
            if (LevelManager.Instance.HandleNonTileTouch(position, rowIndex))
                GameManager.Instance.GameOver();
        }

        else
        {
            // Debug.Log("Not on UI");
        }
    }
}
