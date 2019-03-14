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
        public int bTID { get; set; }

        public Rectangle NextStep { get; set; }
        public bool IsMoving { get; set; }
        public Vector2 Origin;

        public Building(int id, Rectangle iRectangle, Texture2D ITexture, BuildingStatus iStatus, BuildingTypeEnum bTID)
        {
            this.Status = iStatus;
            this.Rectangle = iRectangle;
            this.Texture = ITexture;
            this.ID = id;
            this.BuildingType = bTID;
            this.Origin = new Vector2(0, 0);
        }
        public Building(int id, int bTID)
        {
            this.ID = id;
            this.bTID = bTID;
        }
        public Building(Rectangle iRectangle, Texture2D ITexture, BuildingStatus iStatus, BuildingTypeEnum bTID)
        {
            this.Status = iStatus;
            this.Rectangle = iRectangle;
            this.Texture = ITexture;
            this.BuildingType = bTID;
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

    }
}
