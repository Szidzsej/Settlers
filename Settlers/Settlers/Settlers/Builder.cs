using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Settlers
{
    public class Builder
    {
        public int PlayerNo { get; set; }
        public Texture2D Texture { get; set; }
        public Rectangle Bounds { get; set; }

        public Rectangle NextStep { get; set; }
        public bool IsMoving { get; set; }

        public Vector2 Position;
        public Vector2 Origin;

        private Building building { get; set; }

        public Builder(Building building)
        {
            this.building = building;
            this.building.Rectangle = new Rectangle(0, 0, Globals.BUILDINGSIZE, Globals.BUILDINGSIZE);

        }
        public Builder()
        {

        }

        public void Update()
        {
            if (this.IsMoving)
            {
                if (this.Bounds.X != this.NextStep.X || this.Bounds.Y != this.NextStep.Y)
                {
                    if (this.Bounds.X != this.NextStep.X)
                    {
                        
                    }
                    if (this.Bounds.Y != this.NextStep.Y)
                    {
                       
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
                        //Position += direction * LinearVelocity;
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
