using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Settlers
{
    /// <summary>
    /// Mező definiálása
    /// </summary>
    public class Tile
    {
        public Rectangle Rectangle { get; set; } //Mező poziciója
        public Texture2D Texture { get; set; } //Mező textúrája
        public TileState State { get; set; } //Mező állapotát
        public Color TileColor { get; set; } //Mező árnyalata

        public Tile(Rectangle Rectangle, Texture2D Texture, TileState State,Color TileColor)
        {
            this.Rectangle = Rectangle;
            this.Texture = Texture;
            this.State = State;
            this.TileColor = TileColor;
        }
        public Tile(Rectangle Rectangle, TileState State, Color TileColor)
        {
            this.Rectangle = Rectangle;
            this.State =State;
            this.TileColor = TileColor;
        }
        /// <summary>
        /// Mező kirajzolása
        /// </summary>
        /// <param name="sprite">Kirajzoláshoz szükséges változó</param>
        public void Draw(SpriteBatch sprite)
        {
            sprite.Draw(this.Texture, this.Rectangle, TileColor);
        }
    }
}
