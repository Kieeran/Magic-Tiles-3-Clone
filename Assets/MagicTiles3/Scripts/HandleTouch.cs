using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandleTouch : MonoBehaviour
{
    void Update()
    {
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

    bool IsPointerOverUI(Vector2 position)
    {
        PointerEventData eventData = new(EventSystem.current)
        {
            position = position
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        if (results.Count > 0)
        {
            foreach (var ui in results)
            {
                if (ui.gameObject.CompareTag("Tile"))
                {
                    Debug.Log("Tile");
                }
            }
        }

        else
        {
            Debug.Log("Not on UI");
        }

        return results.Count > 0;
    }
}
