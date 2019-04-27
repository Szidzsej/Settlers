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
