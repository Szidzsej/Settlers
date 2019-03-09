using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Settlers
{
    public enum GameState
    {
        Menu = 0,
        Playing = 1,
        Pause = 2,
        Exit = 3
    }
    public enum Direction
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3
    }
    public enum TileState
    {
        Grass = 0,
        Tree = 1,
        Stone = 2,
        Menu = 3,
        Building = 4
    }
    public enum BuildingStatus
    {
        Placing = 0,
        Construction = 1,
        Ready = 2
    }
    public enum BuildingTypeEnum
    {
        House = 0,
        Woodcutter = 1,
        Stonequarry = 2,
        Wheatfarm = 3,
        Windmill = 4,
        Bakery = 5,
        Hunter = 6,
        Well = 7
    }
}
