using UnityEngine;

public enum TileType
{
    Long,
    Short
}

[CreateAssetMenu(fileName = "NewTile", menuName = "Tiles/Tile Data")]
public class Tile_SO : ScriptableObject
{
    public TileType TileType;
    public int StepIndex;
    public float LongLength;
    public int RowIndex;
}