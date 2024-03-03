using Microsoft.Xna.Framework;
using System.Collections.Generic;


namespace GameCode;

public class Tile
{
    public int X { get; set; }
    public int Y { get; set; }
    public string SpriteName { get; set; }
    public int Option { get; set; }
    public Dictionary<string, string> Data { get; set; } = new Dictionary<string, string>();
}

public class Actor : Tile
{
    public MovementType MovementType { get; set; }

    public Alignment Alignment { get; set; }
}



public enum MovementType
{
    Static,
    PlayerControlled,
    LineOfSight,
    Pathfinding,
}

public enum Alignment
{
    Good,
    Neutral,
    Bad
}
