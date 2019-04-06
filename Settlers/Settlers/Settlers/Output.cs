using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Settlers
{
    class Output
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Origin { get; set; }
        public Color Color { get; set; }
        public SpriteFont FontStyle { get; set; }
        public bool IsItWorker { get; set; }

        public Output(string iName, string iText,  Vector2 iPosition, Color iColor, SpriteFont iFontStyle,bool worker)
        {
            this.Name = iName;
            this.Text = iText;
            this.Position = iPosition;
            this.Color = iColor;
            this.FontStyle = iFontStyle;
            this.IsItWorker = worker;
            //this.Origin = FontStyle.MeasureString(Text) / 2;
        }
        public void Draw(SpriteBatch sprite)
        {
            sprite.DrawString(this.FontStyle, this.Text, this.Position, this.Color,0, new Vector2(0,0), 1.0f, SpriteEffects.None, 0.5f);
        }
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
