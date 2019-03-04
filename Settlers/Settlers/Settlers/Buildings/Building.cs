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
        public int ID {get; set;}
        public int BuildingTypeID { get; set; }
        public int Coordinate { get; set; }
        public Rectangle Rectangle { get; set; }
        public Texture2D Texture { get; set; }
        public Rectangle Bounds { get; set; }

        public Rectangle NextStep { get; set; }
        public bool IsMoving { get; set; }

        public Building(int id, Rectangle iRectangle, Texture2D ITexture, int bTID, int coor)
        {
            this.Rectangle = iRectangle;
            this.Texture = ITexture;
            this.ID = id;
            this.BuildingTypeID = bTID;
            this.Coordinate = coor;
        }
        public Building(int id,  int bTID, int coor)
        {
            this.ID = id;
            this.BuildingTypeID = bTID;
            this.Coordinate = coor;
        }
        public Building(Rectangle iRectangle, Texture2D ITexture, int bTID,int coor)
        {
            this.Rectangle = iRectangle;
            this.Texture = ITexture;
            this.BuildingTypeID = bTID;
            this.Coordinate = coor;
        }
        public void Draw(SpriteBatch sprite)
        {
            sprite.Draw(this.Texture, this.Rectangle, Color.White);
        }
        public void Update()
        {
            if (this.IsMoving)
            {
                if (this.Bounds.X != this.NextStep.X || this.Bounds.Y != this.NextStep.Y)
                {
                    if (this.Bounds.X != this.NextStep.X)
                    {
                        if (this.Bounds.X + Globals.BUILDINGSIZE < this.NextStep.X + Globals.BUILDINGSIZE)
                        {
                            this.Bounds = new Rectangle(this.Bounds.X + Globals.BUILDINGSIZE, this.Bounds.Y, this.Bounds.Width, this.Bounds.Height);
                        }
                        else if (this.Bounds.X + Globals.BUILDINGSIZE > this.NextStep.X - Globals.BUILDINGSIZE)
                        {
                            this.Bounds = new Rectangle(this.Bounds.X - Globals.BUILDINGSIZE, this.Bounds.Y, this.Bounds.Width, this.Bounds.Height);
                        }
                    }
                    if (this.Bounds.Y != this.NextStep.Y)
                    {
                        if (this.Bounds.Y + Globals.BUILDINGSIZE < this.NextStep.Y + Globals.BUILDINGSIZE)
                        {
                            this.Bounds = new Rectangle(this.Bounds.X, this.Bounds.Y + Globals.BUILDINGSIZE, this.Bounds.Width, this.Bounds.Height);
                        }
                        else if (this.Bounds.Y + Globals.BUILDINGSIZE > this.NextStep.Y - Globals.BUILDINGSIZE)
                        {
                            this.Bounds = new Rectangle(this.Bounds.X, this.Bounds.Y - Globals.BUILDINGSIZE, this.Bounds.Width, this.Bounds.Height);
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
