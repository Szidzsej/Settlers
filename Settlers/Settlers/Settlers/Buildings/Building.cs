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
        public int ID { get; set; }
        public BuildingTypeEnum BuildingType { get; set; }
        public Rectangle Rectangle { get; set; }
        public Texture2D Texture { get; set; }
        public Rectangle Bounds { get; set; }
        public BuildingStatus Status { get; set; }
        public bool HasWorker { get; set; }
        public Production Production { get; set; }
        public bool IsItEmpty { get; set; }
        public int WoodStoneCount { get; set; }

        public int BuildTime { get; set; }
        public Rectangle NextStep { get; set; }
        public bool IsMoving { get; set; }
        public Vector2 Origin;

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
        public void UpdateBuidlingStatus(int ms)
        {
            BuildTime += ms;
            if (BuildTime>=Globals.CREATEBUILDINGTIME)
            {
                this.Status = BuildingStatus.Ready;
               
            }
        }
        public void UpdateWorkerStatus(int workers)
        {
            if (workers > 0  && this.BuildingType != BuildingTypeEnum.House)
            {
                HasWorker = true;
            }
        }

        public void Move(Direction iDirection)
        {
            this.Bounds = Step(iDirection);
        }

        public void AnimatedUpdate(Direction iDirection)
        {
            if (!this.IsMoving)
            {
                this.NextStep = Step(iDirection);
                this.IsMoving = true;
            }
            var a = Step(iDirection);
        }
        public void MoveBuilding(Direction iDirection)
        {
            AnimatedUpdate(iDirection);
        }

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
