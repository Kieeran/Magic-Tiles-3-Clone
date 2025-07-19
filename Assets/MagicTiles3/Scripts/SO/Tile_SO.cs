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
    public float StepIndex;
    public float StepLength;
    public int RowIndex;
}