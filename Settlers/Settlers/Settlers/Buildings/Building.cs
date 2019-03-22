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
        }
        public Building(int id, BuildingTypeEnum bTID, Rectangle iRectangle, BuildingStatus iStatus)
        {
            this.ID = id;
            this.BuildingType = bTID;
            this.Rectangle = iRectangle;
            this.Status = iStatus;
        }
        public Building(Rectangle iRectangle, Texture2D ITexture, BuildingStatus iStatus, BuildingTypeEnum bTID, bool iHasWorker)
        {
            this.Status = iStatus;
            this.Rectangle = iRectangle;
            this.Texture = ITexture;
            this.BuildingType = bTID;
            this.HasWorker = iHasWorker;
        }

        public void Draw(SpriteBatch sprite)
        {
            sprite.Draw(this.Texture, new Rectangle(this.Bounds.X, this.Bounds.Y, Globals.BUILDINGSIZE, Globals.BUILDINGSIZE), null, Color.White, 0, this.Origin, SpriteEffects.None, 0f);
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
