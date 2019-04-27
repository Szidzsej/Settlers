using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Settlers
{
    /// <summary>
    /// Szövegek definiálása
    /// </summary>
    class Output
    {
        public string Name { get; set; } // Szöveg neve
        public string Text { get; set; } // Szöveg tartalma
        public Vector2 Position { get; set; } // Szöveg poziciója
        public Vector2 Origin { get; set; } // Szöveg középpontja
        public Color Color { get; set; } // Szöveg színe
        public SpriteFont FontStyle { get; set; } // Szöveg betű típusa
        public bool IsItWorker { get; set; } // Szöveg a munkások számát írja-e ki

        public Output(string iName, string iText,  Vector2 iPosition, Color iColor, SpriteFont iFontStyle,bool worker)
        {
            this.Name = iName;
            this.Text = iText;
            this.Position = iPosition;
            this.Color = iColor;
            this.FontStyle = iFontStyle;
            this.IsItWorker = worker;
        }
        /// <summary>
        /// Szöveg kirajzolása
        /// </summary>
        /// <param name="sprite"></param>
        public void Draw(SpriteBatch sprite)
        {
            sprite.DrawString(this.FontStyle, this.Text, this.Position, this.Color,0, new Vector2(0,0), 1.0f, SpriteEffects.None, 0.5f);
        }
        /// <summary>
        /// SZövegek frissítése
        /// </summary>
        /// <param name="bM">Nyersanyag</param>
        /// <param name="value"> Nyersanyag mennyiség</param>
        /// <param name="actualWorkers"> Elérhető munkások száma</param>
        /// <param name="allWorkers"> Az összes munkás száma</param>
        public void Update(BaseMaterial bM, int value,int actualWorkers, int allWorkers)
        {
            if (IsItWorker)
            {
                this.Text = $"Workers: {actualWorkers}/{allWorkers}";
            }
            else
            {
                this.Text = $"{bM.Name}: {value}";
            }
        }
    }
}
