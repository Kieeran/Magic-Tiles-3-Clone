using UnityEngine;

public enum TileType
{
    LongTile,
    ShortTile
}

[CreateAssetMenu(fileName = "NewTile", menuName = "Tiles/Tile Data")]
public class Tile_SO : ScriptableObject
{
    public TileType TileType;
    public float LongLength;
    public float AppearTime;
    public int RowIndex;
}