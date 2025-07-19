using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvas : MonoBehaviour
{
    public Transform GameOverPopUp;

    void Awake()
    {

    }

    void Start()
    {
        GameManager.Instance.OnGameOver += () =>
        {
            GameOverPopUp.gameObject.SetActive(true);
        };
    }
}
