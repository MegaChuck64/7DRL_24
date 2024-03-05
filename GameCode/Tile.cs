using Microsoft.Xna.Framework;
using System.Collections.Generic;


namespace GameCode;

public class Tile
{
    public int X { get; set; }
    public int Y { get; set; }
    public string SpriteName { get; set; }
    public int Option { get; set; }
    public List<string> Data { get; set; } = new List<string>();
}

public class Actor : Tile
{
    public string Name { get; set; }
    public int Health { get; set; }

}


