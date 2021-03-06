﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Settlers
{
    public class Map
    {
        public List<Tile> Tiles { get; set; }
        public List<Button> BuildingButtons;
        #region Koordináta segéd változók
        Random random = new Random();
        int yCoor = 0;
        int xCoor = 0;
        int startYCoor=0;
        int endYCoor=0;
        int startXCoor =0;
        int endXCoor=0;
        #endregion

        #region Map vége koordináta
        private int xMapEnd = 59*Globals.TILESIZE;
        #endregion
        

        public Map()
        {
            this.Tiles = new List<Tile>();
            this.BuildingButtons = new List<Button>();

        }
        private int[,] MapCreator()
        {
            int[,] map = new int[40, 60];
            var a = File.ReadAllLines("nullMap.txt");

            int row = 0;
            foreach (var x in a)
            {
                int col = 0;

                var splitted = x.Split(';').Where(y => !string.IsNullOrEmpty(y));
                foreach (var y in splitted)
                {
                    map[row, col] = int.Parse(y);
                    col++;
                }
                row++;
            }
            TreeSpawner(map);
            StoneSpawner(map);
            TreeSpawner(map);
            return map;
        }
        private int[,] TreeSpawner(int[,] map)
        {
            yCoor = random.Next(5, Globals.TILEROWCOUNT - 5);
            xCoor = random.Next(5, Globals.TILECOLUMNCOUNT - 5);
            startYCoor = yCoor - 5;
            endYCoor = yCoor + 5;
            startXCoor = xCoor;
            endXCoor = xCoor;

            for (int y = startYCoor; y <= endYCoor; y++)
            {
                for (int x = startXCoor; x <= endXCoor; x++)
                {
                    if ((x >= xCoor - 2 && xCoor + 2 >= x) && (y >= yCoor - 2 && yCoor + 2 >= y))
                    {
                        map[y, x] = 1;
                    }
                    else
                    {
                        map[y, x] = random.Next(0, 2);
                    }
                }
                if (y < yCoor)
                {
                    startXCoor--;
                    endXCoor++;
                }
                else
                {
                    startXCoor++;
                    endXCoor--;
                }
            }
            return map;
        }
        private int[,] StoneSpawner(int[,] map)
        {
            yCoor = random.Next(5, Globals.TILEROWCOUNT - 5);
            xCoor = random.Next(5, Globals.TILECOLUMNCOUNT - 5);

            startYCoor = yCoor - 5;
            endYCoor = yCoor + 5;
            startXCoor = xCoor;
            endXCoor = xCoor;
            for (int y = startYCoor; y <= endYCoor; y++)
            {
                for (int x = startXCoor; x <= endXCoor; x++)
                {
                    if ((x >= xCoor - 2 && xCoor + 2 >= x) && (y >= yCoor - 2 && yCoor + 2 >= y))
                    {
                        map[y, x] = 2;
                    }
                    else
                    {
                        map[y, x] = random.Next(0, 2)*2;
                    }
                }
                if (y < yCoor)
                {
                    startXCoor--;
                    endXCoor++;
                }
                else
                {
                    startXCoor++;
                    endXCoor--;
                }
            }
            return map;
        }
        public void InitTiles(Texture2D grass, Texture2D tree, Texture2D stone,Texture2D buildingMenu)
        {
            var a = MapCreator();
            for (int i = 0; i < 40; i++)
            {
                for (int j = 0; j < 60; j++)
                {
                    Tiles.Add(new Tile(
                    new Rectangle((j * Globals.TILESIZE), (i * Globals.TILESIZE), Globals.TILESIZE, Globals.TILESIZE),
                    (a[i, j] == 0) ? grass : (a[i, j] == 1) ? tree : stone,
                    (a[i, j] == 0) ? TileState.Grass : (a[i, j] == 1) ? TileState.Tree : TileState.Stone));
                }
            }
            Tiles.Add(new Tile(new Rectangle(xMapEnd + Globals.TILESIZE, 0, 200, 600), buildingMenu, TileState.Menu));

        }

        public bool CheckTile(Direction iDirection, Rectangle a)
        {
            List<Tile> tileHelp = this.Tiles.FindAll(x => ((x.Rectangle.X == a.X) && (x.Rectangle.Y == a.Y)) || (((a.X + Globals.TILESIZE) == x.Rectangle.X) && a.Y == x.Rectangle.Y)
              || (((a.X + Globals.TILESIZE) == x.Rectangle.X) && ((a.Y + Globals.TILESIZE) == x.Rectangle.Y)) || ((a.X == x.Rectangle.X) && ((a.Y + Globals.TILESIZE) == x.Rectangle.Y)));
            
            if (tileHelp.FindAll(x=>x.State == TileState.Grass).Count()==4)
            {
                return true;
            }

            return false;
        }

        public List<Tile> InitLoadedTiles(Texture2D grass, Texture2D tree, Texture2D stone, Texture2D buildingMenu)
        {
            foreach (var tile in Tiles)
            {
                if (tile.State == TileState.Tree)
                {
                    tile.Texture = tree;
                }
                else
                {
                    if (tile.State == TileState.Grass || tile.State == TileState.Building)
                    {
                        tile.Texture = grass;
                    }
                    else
                    {
                        if (tile.State == TileState.Stone)
                        {
                            tile.Texture = stone;
                        }
                        else
                        {
                            tile.Texture = buildingMenu;
                        }
                    }
                }
            }
            Tiles.Add(new Tile(new Rectangle(xMapEnd + Globals.TILESIZE, 0, 200, 600), buildingMenu, TileState.Menu));
            return this.Tiles;
        }

        public void PlaceBuilding(Rectangle r)
        {
            int xCoorStart = 0;
            int yCoorStart = 0;   
            xCoorStart = r.X;
            yCoorStart = r.Y;
             
            List<Tile> tileHelp = new List<Tile>();
            foreach (var item in Tiles)
            {
                if ((item.Rectangle.X == xCoorStart && item.Rectangle.Y == yCoorStart) || (item.Rectangle.X == (xCoorStart + Globals.TILESIZE) && item.Rectangle.Y == yCoorStart) ||
                    (item.Rectangle.X == xCoorStart && item.Rectangle.Y == (yCoorStart + Globals.TILESIZE)) || (item.Rectangle.X == (xCoorStart + Globals.TILESIZE) && item.Rectangle.Y == (yCoorStart + Globals.TILESIZE)))
                {
                    tileHelp.Add(item);
                }
            }
            foreach (var item in tileHelp)
            {
                item.State = TileState.Building;
            }

        }
        public List<Button> InitInGameMenu(Dictionary<string,Texture2D> Textures)
        {
            int yHelper = 60;
            int countHelper = 1;
            string s = null;
            foreach (var item in Textures)
            {
                if (item.Key.Contains("_"))
                {
                    s = item.Key.Substring(6);
                    if (countHelper % 2 == 1)
                    {
                        
                        BuildingButtons.Add(new Button((xMapEnd + (Globals.MENUICON + 80)), yHelper, Globals.MENUICON, Globals.MENUICON,Textures[s], item.Value, GetGameMenuBuildingType(s)));
                    }
                    else
                    {
                        BuildingButtons.Add(new Button(xMapEnd + 30, yHelper, Globals.MENUICON, Globals.MENUICON, Textures[s], item.Value, GetGameMenuBuildingType(s)));
                        yHelper += 90;
                    }
                }   
                countHelper++;
            }
            return BuildingButtons;
        }
        private BuildingTypeEnum GetGameMenuBuildingType(string s)
        {
            BuildingTypeEnum type;
            switch (s)
            {
                case "bakery" : type = BuildingTypeEnum.Bakery;  break;
                case "house": type = BuildingTypeEnum.House; break;
                case "hunter": type = BuildingTypeEnum.Hunter; break;
                case "stonequarry": type = BuildingTypeEnum.Stonequarry; break;
                case "wheatfarm": type = BuildingTypeEnum.Wheatfarm; break;
                case "well": type = BuildingTypeEnum.Well; break;
                case "woodcutter": type = BuildingTypeEnum.Woodcutter; break;
                case "windmill": type = BuildingTypeEnum.Windmill; break;
                    default : type = BuildingTypeEnum.House; break;
            }
            return type;
        }

        public void Update(MouseState ms,MouseState prevMS, List<Button> GameMenuButtons, Dictionary<string,Texture2D> Textures)
        {
                   
        }

        public void Draw(SpriteBatch sprite)
        {
            this.Tiles.ForEach(x =>
            {
                x.Draw(sprite);
            });
            this.BuildingButtons.ForEach(x =>
            {
                x.Draw(sprite);
            });
        }
    }
}

