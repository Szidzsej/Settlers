using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Settlers
{
    public class Tile
    {
        public Rectangle Rectangle { get; set; }
        public Texture2D Texture { get; set; }
        public bool State { get; set; }

        public Tile(Rectangle iRectangle, Texture2D ITexture, bool iState)
        {
            this.Rectangle = iRectangle;
            this.Texture = ITexture;
            this.State = iState;
        }

        public void Draw(SpriteBatch sprite)
        {
            sprite.Draw(this.Texture, this.Rectangle, Color.White);
        }
    }
}
