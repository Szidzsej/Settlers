using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Settlers
{
    public class Button
    {
        private Texture2D texture;          // Aktuális textúra
        private Texture2D texture_state1;   // Textúra 1-es állapota
        private Texture2D texture_state2;   // Textúra 2-es állapota
        private int w, h;                   // Szélesség, magasság
        private Vector2 pos;                // Pozíció

        public float X { get { return pos.X; } set { pos.X = value; } }
        public float Y { get { return pos.Y; } set { pos.Y = value; } }
        public int W { get { return w; } set { w = value; } }
        public int H { get { return h; } set { h = value; } }
        public Texture2D Texture { get { return texture; } set { texture = value; } }
        public Texture2D TextureState2 { get { return texture_state2; } }
        public Texture2D TextureState1 { get { return texture_state1; } }

        public Button(int x, int y)
        {
            this.pos = new Vector2(x, y);
        }

        public Button(int x, int y, Texture2D state1, Texture2D state2)
        {
            this.pos = new Vector2(x, y);
            this.texture = state1;
            this.w = state1.Width;
            this.h = state1.Height;
            this.texture_state1 = state1;
            this.texture_state2 = state2;
        }

        public Button(int x, int y, int w, int h, Texture2D state1, Texture2D state2)
        {
            this.pos = new Vector2(x, y);
            this.texture = state1;
            this.w = w;
            this.h = h;
            this.texture_state1 = state1;
            this.texture_state2 = state2;
        }

        public Button(int x, int y, int w, int h)
        {
            this.pos = new Vector2(x, y);
            this.w = w;
            this.h = h;
        }

        public void Init(Texture2D texture)
        {
            this.texture = texture;
            this.w = texture.Width;
            this.h = texture.Height;
        }
        public void InitPos(Vector2 pos)
        {
            this.pos = pos;
        }

        public void ChangeState(int stateID)
        {
            if (stateID == 1)
            {
                if (this.Texture != this.TextureState1)
                    this.Texture = this.TextureState1;
            }
            else if (stateID == 2)
            {
                if (this.Texture != this.TextureState2)
                    this.Texture = this.TextureState2;
            }
        }

        public bool LeftClick(MouseState m, MouseState pm)
        {
            if (m.X >= pos.X && m.X <= pos.X + w && m.Y >= pos.Y && m.Y <= pos.Y + h && m.LeftButton == ButtonState.Pressed && pm.LeftButton == ButtonState.Released)
                return true;
            else
                return false;
        }
        public bool RightClick(MouseState m, MouseState pm)
        {
            if (m.X >= pos.X && m.X <= pos.X + w && m.Y >= pos.Y && m.Y <= pos.Y + h && m.RightButton == ButtonState.Pressed && pm.RightButton == ButtonState.Released)
                return true;
            else
                return false;
        }
        public bool MouseOver(MouseState m)
        {
            if (m.X >= pos.X && m.X <= pos.X + w && m.Y >= pos.Y && m.Y <= pos.Y + h)
                return true;
            else
                return false;
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, new Rectangle((int)this.pos.X, (int)this.pos.Y, (int)w, (int)h), Color.White);
        }

        public void DrawText(SpriteBatch spritebatch, SpriteFont font, Vector2 where, string text)
        {
            spritebatch.DrawString(font, text, where, Color.White);
        }
    }
}
