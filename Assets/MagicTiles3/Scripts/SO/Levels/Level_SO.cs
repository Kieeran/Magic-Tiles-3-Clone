using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Levels/Level Data")]
public class Level_SO : ScriptableObject
{
    public List<Tile_SO> Tiles;
}
