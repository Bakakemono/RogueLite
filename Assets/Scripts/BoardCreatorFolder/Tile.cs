using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {

    public TileType tileType;
    public Vector2Int TilePosition;


    public enum TileType
    {
        Wall, Floor,
    }

    public Tile(Vector2Int _position, TileType _tileType)
    {
        TilePosition = _position;
        tileType = _tileType;
    }
}
