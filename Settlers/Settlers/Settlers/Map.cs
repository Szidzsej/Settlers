using Microsoft.Xna.Framework;
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
        public List<Building> Buildings;
        int bID = 0;
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
            this.Buildings = new List<Building>();

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
        public void MoveBuilding(Direction iDirection, int iID)
        {
            var a = Buildings.FirstOrDefault(x => x.ID == iID);
            if (CheckTile(iDirection, iID))
                a.AnimatedUpdate(iDirection);
        }

        private bool CheckTile(Direction iDirection, int iID)
        {
            var a = Buildings.FirstOrDefault(x => x.ID == iID).Step(iDirection);

            if (this.Buildings.FindAll(x => x.ID != iID).Any(x => x.Bounds.X == a.X && x.Bounds.Y == a.Y))
            {
                return true;
            }
            List<Tile> tileHelp = this.Tiles.FindAll(x => ((x.Rectangle.X == a.X) && (x.Rectangle.Y == a.Y)) || (((a.X + Globals.TILESIZE) == x.Rectangle.X) && a.Y == x.Rectangle.Y)
              || (((a.X + Globals.TILESIZE) == x.Rectangle.X) && ((a.Y + Globals.TILESIZE) == x.Rectangle.Y)) || ((a.X == x.Rectangle.X) && ((a.Y + Globals.TILESIZE) == x.Rectangle.Y)));
            foreach (var item in tileHelp)
            {
                if (4 != (int)item.State)
                {
                    return true;
                }
            }


            return false;
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
                        BuildingButtons.Add(new Button((xMapEnd + (Globals.MENUICON + 80)), yHelper, Globals.MENUICON, Globals.MENUICON,Textures[s], item.Value));
                    }
                    else
                    {
                        BuildingButtons.Add(new Button(xMapEnd + 30, yHelper, Globals.MENUICON, Globals.MENUICON, Textures[s], item.Value));
                        yHelper += 90;
                    }
                }   
                countHelper++;
            }
            return BuildingButtons;
        }

        public void Update(MouseState ms,MouseState prevMS, List<Button> GameMenuButtons, Dictionary<string,Texture2D> Textures)
        {
            string[] s = null;
            foreach (var item in GameMenuButtons)
            {
                s = Textures.FirstOrDefault(x => x.Value == item.Texture).Key.Split('_');
                if (item.MouseOver(ms)) { item.ChangeState(2); } else { item.ChangeState(1); }
                if (item.LeftClick(ms,prevMS))
                {
                    Buildings.Add(new Building(bID,new Rectangle(0, 0, Globals.BUILDINGSIZE, Globals.BUILDINGSIZE), Textures[s[1]],0,0));
                    bID++;
                }
            }
            this.Buildings.ForEach(x => x.Update());
                   
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
            this.Buildings.ForEach(x =>
            {
                x.Draw(sprite);
            });
        }
    }
}

