using Microsoft.Xna.Framework;
using System.Collections.Generic;


namespace GameCode;

public class Tile
{
    public int X { get; set; }
    public int Y { get; set; }
    public string SpriteName { get; set; }
    public int Option { get; set; }
    public bool Collider { get; set; }
}

public class Actor : Tile
{
    public int Health { get; set; }

    public Dictionary<string, Item> Inventory { get; set; } = new Dictionary<string, Item>();

    public int SelectedItem { get; set; }    

    public int SightRange { get; set; }

    public int Strength { get; set; }

}

public class Item : Tile
{
    public string Description { get; set; }
    public bool Collectable { get; set; }

    public int Count { get; set; } = 1;
}

public class Weapon : Item
{
    public List<string> Actions { get; set; }
}

public class Monster : Actor
{

}



