using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Settlers
{
    /// <summary>
    /// Gombok definiálása
    /// </summary>
    public class Button
    {
        private Texture2D texture;  // Gomb textúrája  (segéd változó)
        private Texture2D texture_state1;   //Gomb textúrája mikor fölé viszük az egeret (segéd változó)
        private Texture2D texture_state2;   // Gomb textúrája lenyomáskor (segéd változó) 
        private int w, h;                   // Gomb szélessége, magassága (segéd változó)
        private Vector2 pos;                // Gomb poziciója (segéd változó)

        public BuildingTypeEnum buildingType { get; set; } // Ha a gomb épülethez tartozik, akkor annak az épület tipusának az enumját tartalmazza
        public float X { get { return pos.X; } set { pos.X = value; } } // Gomb X koordinátája
        public float Y { get { return pos.Y; } set { pos.Y = value; } } // Gomb Y koordinátája
        public int W { get { return w; } set { w = value; } } // Gomb szélessége
        public int H { get { return h; } set { h = value; } } // Gomb magassága
        public Texture2D Texture { get { return texture; } set { texture = value; } } // Gomb textúrája
        public Texture2D TextureState2 { get { return texture_state2; } } // Gomb textúrája lenyomáskor
        public Texture2D TextureState1 { get { return texture_state1; } } // Gomb textúrája mikor fölé viszük az egeret

        public Button(int x, int y)
        {
            this.pos = new Vector2(x, y);
        }

        public Button(int x, int y, int w, int h, Texture2D state1, Texture2D state2, BuildingTypeEnum bType)
        {
            this.pos = new Vector2(x, y);
            this.texture = state1;
            this.w = w;
            this.h = h;
            this.texture_state1 = state1;
            this.texture_state2 = state2;
            this.buildingType = bType;
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
        /// <summary>
        /// Gomb inicializálása
        /// </summary>
        /// <param name="texture">Gomb textúrája</param>
        public void Init(Texture2D texture)
        {
            this.texture = texture;
            this.w = texture.Width;
            this.h = texture.Height;
        }
        // Gomb poziciójának inicializálása
        public void InitPos(Vector2 pos)
        {
            this.pos = pos;
        }
        /// <summary>
        /// Gomb állapotának megváltoztatása
        /// </summary>
        /// <param name="stateID">Jelenlegi állapot azonosítója</param>
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
        /// <summary>
        /// Gombra való bal kattintás inicializálása
        /// </summary>
        /// <param name="m"></param>
        /// <param name="pm"></param>
        /// <returns></returns>
        public bool LeftClick(MouseState m, MouseState pm)
        {
            if (m.X >= pos.X && m.X <= pos.X + w && m.Y >= pos.Y && m.Y <= pos.Y + h && m.LeftButton == ButtonState.Pressed && pm.LeftButton == ButtonState.Released)
                return true;
            else
                return false;
        }
        /// <summary>
        /// Gomb vizsgálata, hogy az egér rajta van-e
        /// </summary>
        /// <param name="m">Egér állapota</param>
        /// <returns></returns>
        public bool MouseOver(MouseState m)
        {
            if (m.X >= pos.X && m.X <= pos.X + w && m.Y >= pos.Y && m.Y <= pos.Y + h)
                return true;
            else
                return false;
        }
        /// <summary>
        /// Kirajzolja a gombot
        /// </summary>
        /// <param name="spritebatch"></param>
        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, new Rectangle((int)this.pos.X, (int)this.pos.Y, (int)w, (int)h), Color.White);
        }
    }
}
