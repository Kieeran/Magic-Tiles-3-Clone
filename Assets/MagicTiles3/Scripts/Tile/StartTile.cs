using UnityEngine;
using UnityEngine.UI;

public class StartTile : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            GameManager.Instance.GameStart();

            Destroy(gameObject);
        });
    }
}
