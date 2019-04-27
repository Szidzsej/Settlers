using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Settlers
{   
    /// <summary>
    /// Játék állapotok
    /// </summary>
    public enum GameState 
    {
        Menu = 0,
        Playing = 1,
        Pause = 2,
        Exit = 3,
        Error = 4,
        BeforePlaying = 5,
        EndGame = 6
    }
    /// <summary>
    /// Irányok
    /// </summary>
    public enum Direction
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3
    }
    /// <summary>
    /// Mező állapotok
    /// </summary>
    public enum TileState
    {
        Grass = 0,
        Tree = 1,
        Stone = 2,
        Menu = 3,
        Building = 4
    }
    /// <summary>
    /// Épület állapotok
    /// </summary>
    public enum BuildingStatus
    {
        Placing = 0,
        Construction = 1,
        Ready = 2
    }
    /// <summary>
    /// Épület tipusok
    /// </summary>
    public enum BuildingTypeEnum
    {
        House = 7,
        Woodcutter = 1,
        Stonequarry = 2,
        Wheatfarm = 3,
        Windmill = 4,
        Bakery = 6,
        Hunter = 8,
        Well = 5
    }
}
