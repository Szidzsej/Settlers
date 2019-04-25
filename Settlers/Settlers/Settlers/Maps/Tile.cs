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
        public TileState State { get; set; }
        public Color TileColor { get; set; }

        public Tile(Rectangle iRectangle, Texture2D ITexture, TileState iState,Color iTileColor)
        {
            this.Rectangle = iRectangle;
            this.Texture = ITexture;
            this.State = iState;
            this.TileColor = iTileColor;
        }
        public Tile(Rectangle iRectangle, TileState iState, Color iTileColor)
        {
            this.Rectangle = iRectangle;
            this.State = iState;
            this.TileColor = iTileColor;
        }

        public void Draw(SpriteBatch sprite)
        {
            sprite.Draw(this.Texture, this.Rectangle, TileColor);
        }
    }
}
