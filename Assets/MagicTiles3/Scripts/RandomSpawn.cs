using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    public Tile TilePrefab;
    public float SpawnCoolDown = 2f;

    List<Transform> _rows;
    float _timer;
    float _ySpawn = 2500f;

    void Start()
    {
        _timer = 0f;
        _rows = new List<Transform>();
        foreach (Transform child in transform)
        {
            _rows.Add(child);
        }
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer < SpawnCoolDown) return;

        _timer = 0;

        int randomIndexRow = Random.Range(0, _rows.Count);
        Tile tile = Instantiate(TilePrefab, _rows[randomIndexRow]);
        tile.transform.localPosition = new Vector3(0, _ySpawn, 0);
        tile.ResetRectTransform();
    }
}
