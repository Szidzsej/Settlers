using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Settlers
{
    public class Building 
    {
        public int ID { get; set; } // Épület azonositoja
        public BuildingTypeEnum BuildingType { get; set; } // Épület Tipusa
        public Rectangle Rectangle { get; set; } // Épület kirajzolásához szükséges kooridnátákat és méreteket tárolja
        public Texture2D Texture { get; set; } // Épület textúrája
        public Rectangle Bounds { get; set; } // Lehelyezett épület pontos helye a pályán
        public BuildingStatus Status { get; set; } // Épület státusza pl. Épités alatt
        public bool HasWorker { get; set; } // Dolgozik-e valaki az épületben
        public Production Production { get; set; } // Épület termelését tárolja 
        public bool IsItEmpty { get; set; } // Üres-e a lakóház
        public int WoodStoneCount { get; set; } // Az épület körzetében lévő fa vagy kő mennyisége

        public int BuildTime { get; set; } // Az idő mig felépül az épület
        public Rectangle NextStep { get; set; } // Az következő lépés, mozgatás sorám
        public bool IsMoving { get; set; } // Mozog-e az épület
        public Vector2 Origin; // Épület középpontja

        public Building(int id, Rectangle iRectangle, Texture2D ITexture, BuildingStatus iStatus, BuildingTypeEnum bTID, bool iHasWorker)
        {
            this.Status = iStatus;
            this.Rectangle = iRectangle;
            this.Texture = ITexture;
            this.ID = id;
            this.BuildingType = bTID;
            this.Origin = new Vector2(0, 0);
            this.HasWorker = iHasWorker;
            this.BuildTime = 0;
            this.IsItEmpty = false;
            this.WoodStoneCount = 0;
        }
        public Building(int id, BuildingTypeEnum bTID, Rectangle iRectangle, BuildingStatus iStatus)
        {
            this.ID = id;
            this.BuildingType = bTID;
            this.Rectangle = iRectangle;
            this.Status = iStatus;
            this.BuildTime = 0;
            this.IsItEmpty = false;
            this.WoodStoneCount = 0;
        }
        public Building(Rectangle iRectangle, Texture2D ITexture, BuildingStatus iStatus, BuildingTypeEnum bTID, bool iHasWorker)
        {
            this.Status = iStatus;
            this.Rectangle = iRectangle;
            this.Texture = ITexture;
            this.BuildingType = bTID;
            this.HasWorker = iHasWorker;
            this.BuildTime = 0;
            this.IsItEmpty = false;
            this.WoodStoneCount = 0;
        }

        /// <summary>
        /// Az épület körzetét adja vissza 
        /// </summary>
        /// <param name="map">Pálya</param>
        /// <returns></returns>
        private List<Tile> BuildingsArea(Map map)
        {
            List<Tile> temp = new List<Tile>();
            int minX = this.Bounds.X - (Globals.TILESIZE*3);
            if (minX < 0)
                minX = 0;
            int minY = this.Bounds.Y - (Globals.TILESIZE * 3);
            if (minY < 0)
                minY = 0;
            int maxX = this.Bounds.X + (Globals.TILESIZE * 4);
            if (maxX > 900)
            {
                maxX = 900;
            }
            int maxY = this.Bounds.Y + (Globals.TILESIZE * 4);
            if (maxY > 600)
            {
                maxY = 600;
            }
            foreach (var item in map.Tiles)
            {
                if (((item.Rectangle.X >= minX )&& (item.Rectangle.X <= maxX)) && ((item.Rectangle.Y >= minY) && (item.Rectangle.Y <= maxY)))
                {
                    temp.Add(item);
                }
            }
            return temp;
        }

        /// <summary>
        /// Kiszinezi az épület körzetét, hogy látható legyen hány fa/kő van a közelben
        /// </summary>
        /// <param name="colorized">Jelenleg kivan-e szinezve</param>
        /// <param name="map">Pálya</param>
        /// <param name="sprite">Kirajzoláshoz szükséges változó</param>
        /// <returns>Vissza a mezőket a közelben</returns>
        public List<Tile> ColorizeProductionArea(bool colorized, Map map,SpriteBatch sprite)
        {
            List<Tile> colorizedTiles = BuildingsArea(map);
            foreach (var item in colorizedTiles)
            {
                if (colorized)
                {
                    item.TileColor = Color.Green;
                }
                else
                {
                    item.TileColor = Color.White;
                }
                
            }
            return colorizedTiles;
        }
        /// <summary>
        /// Kiszámolja hány fa/kő van a közelben
        /// </summary>
        /// <param name="map">Pálya</param>
        public void ProductionSum(Map map)
        {
            List<Tile> temp = BuildingsArea(map);
            if (temp != null && temp.Count() > 0)
            {
                foreach (var item in temp)
                {
                    if (this.BuildingType == BuildingTypeEnum.Woodcutter)
                    {
                        if (item.State == TileState.Tree)
                        {
                            WoodStoneCount++;
                        }
                    }
                    if (this.BuildingType == BuildingTypeEnum.Stonequarry)
                    {
                        if (item.State == TileState.Stone)
                        {
                            WoodStoneCount++;
                        }
                    }
                }
            }
            else
                this.WoodStoneCount = 0;
        }
        /// <summary>
        /// Épület kirajzolása
        /// </summary>
        /// <param name="sprite">Kirajzoláshoz szükséges változó</param>
        public void Draw(SpriteBatch sprite)
        {
            if (this.Status == BuildingStatus.Construction)
            {
                sprite.Draw(this.Texture, new Rectangle(this.Bounds.X, this.Bounds.Y, Globals.BUILDINGSIZE, Globals.BUILDINGSIZE), null, Color.Red, 0, this.Origin, SpriteEffects.None, 0f);
            }
            else
            {
                if (!HasWorker && this.Status == BuildingStatus.Ready && this.BuildingType != BuildingTypeEnum.House)
                {
                    sprite.Draw(this.Texture, new Rectangle(this.Bounds.X, this.Bounds.Y, Globals.BUILDINGSIZE, Globals.BUILDINGSIZE), null, Color.Yellow, 0, this.Origin, SpriteEffects.None, 0f);
                }
                else
                {
                    sprite.Draw(this.Texture, new Rectangle(this.Bounds.X, this.Bounds.Y, Globals.BUILDINGSIZE, Globals.BUILDINGSIZE), null, Color.White, 0, this.Origin, SpriteEffects.None, 0f);
                }
            }
        }
        /// <summary>
        /// Épület frissitése
        /// </summary>
        public void Update()
        {
            if (this.IsMoving && this.Status == BuildingStatus.Placing)
            {
                if (this.Bounds.X != this.NextStep.X || this.Bounds.Y != this.NextStep.Y)
                {
                    if (this.Bounds.X != this.NextStep.X)
                    {
                        if ((this.Bounds.X + Globals.TILESIZE < this.NextStep.X + Globals.TILESIZE))
                        {
                            this.Bounds = new Rectangle(this.Bounds.X + Globals.TILESIZE, this.Bounds.Y, this.Bounds.Width, this.Bounds.Height);
                        }
                        else if (this.Bounds.X + Globals.TILESIZE > this.NextStep.X - Globals.TILESIZE)
                        {
                            this.Bounds = new Rectangle(this.Bounds.X - Globals.TILESIZE, this.Bounds.Y, this.Bounds.Width, this.Bounds.Height);
                        }
                    }
                    if ((this.Bounds.Y != this.NextStep.Y))
                    {
                        if (this.Bounds.Y + Globals.TILESIZE < this.NextStep.Y + Globals.TILESIZE)
                        {
                            this.Bounds = new Rectangle(this.Bounds.X, this.Bounds.Y + Globals.TILESIZE, this.Bounds.Width, this.Bounds.Height);
                        }
                        else if (this.Bounds.Y + Globals.TILESIZE > this.NextStep.Y - Globals.TILESIZE)
                        {
                            this.Bounds = new Rectangle(this.Bounds.X, this.Bounds.Y - Globals.TILESIZE, this.Bounds.Width, this.Bounds.Height);
                        }
                    }
                }
                else
                    this.IsMoving = false;
            }
        }
        /// <summary>
        /// Épület állapotának megvizsgálása, felépült-e
        /// </summary>
        /// <param name="ms">Eltelt idő</param>
        public void UpdateBuidlingStatus(int ms)
        {
            BuildTime += ms;
            if (BuildTime>=Globals.CREATEBUILDINGTIME)
            {
                this.Status = BuildingStatus.Ready;
               
            }
        }
        /// <summary>
        /// Ha még nincs az épületnek munkása, megvizsgálja van-e elérhető szabad munkás, ha van hozzá füzi az épülethez
        /// </summary>
        /// <param name="workers">Összes szabad munkás száma</param>
        public void UpdateWorkerStatus(int workers)
        {
            if (workers > 0  && this.BuildingType != BuildingTypeEnum.House)
            {
                HasWorker = true;
            }
        }

        /// <summary>
        /// Épület mozgatása
        /// </summary>
        /// <param name="iDirection">Milyen irányba mozog</param>
        public void Move(Direction iDirection)
        {
            this.Bounds = Step(iDirection);
        }

        /// <summary>
        /// Az épület mozgásának az animációja
        /// </summary>
        /// <param name="iDirection">Melyik irányba mozog</param>
        public void AnimatedUpdate(Direction iDirection)
        {
            if (!this.IsMoving)
            {
                this.NextStep = Step(iDirection);
                this.IsMoving = true;
            }
            var a = Step(iDirection);
        }

        /// <summary>
        /// Az épület tényleges mozgatása, mikor már le lett vizsgálva tud-e az adott irányba mozogni
        /// </summary>
        /// <param name="iDirection">Melyik irányba mozog</param>
        public void MoveBuilding(Direction iDirection)
        {
            AnimatedUpdate(iDirection);
        }

        /// <summary>
        /// A kövezkező lépés definiálása
        /// </summary>
        /// <param name="iDirection">Melyik irányba mozog</param>
        /// <returns>Vissza adja a lépés helyének, koordinátáját</returns>
        public Rectangle Step(Direction iDirection)
        {
            switch (iDirection)
            {
                case Direction.Up:
                    {
                        return new Rectangle(this.Bounds.X, this.Bounds.Y - Globals.TILESIZE, this.Bounds.Width, this.Bounds.Height);
                    }
                case Direction.Right:
                    {
                        return new Rectangle(this.Bounds.X + Globals.TILESIZE, this.Bounds.Y, this.Bounds.Width, this.Bounds.Height);
                    }
                case Direction.Down:
                    {
                        return new Rectangle(this.Bounds.X, this.Bounds.Y + Globals.TILESIZE, this.Bounds.Width, this.Bounds.Height);
                    }
                case Direction.Left:
                    {
                        return new Rectangle(this.Bounds.X - Globals.TILESIZE, this.Bounds.Y, this.Bounds.Width, this.Bounds.Height);
                    }
                default:
                    return Rectangle.Empty;
            }
        }

        /// <summary>
        /// Az épület tipusának az konvertálása, szöveggé
        /// </summary>
        /// <param name="bTID">Az épület tipusának azonositója</param>
        /// <returns>Vissza adja az épület tipusát szövegként</returns>
        public string GetTextureName(BuildingTypeEnum bTID)
        {
            string s = "";
            switch (bTID)
            {
                case BuildingTypeEnum.Bakery: s=  "bakery"; break;
                case BuildingTypeEnum.House:  s = "house"; break;
                case BuildingTypeEnum.Hunter: s = "hunter" ; break;
                case BuildingTypeEnum.Stonequarry: s = "stonequarry" ; break;
                case BuildingTypeEnum.Wheatfarm: s = "wheatfarm"  ; break;
                case BuildingTypeEnum.Well: s = "well"  ; break;
                case BuildingTypeEnum.Woodcutter: s = "woodcutter"  ; break;
                case BuildingTypeEnum.Windmill: s = "windmill" ; break;
                default: s = "house"; break;
            }
            return s;
        }

        
    }
}
