using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neuralNetwork_01_upg_3.Simulator.Game.Snake
{
    public class GameTex
    {
        public Texture2D tex;
        public Color color;
        public Color color2;
        public Rectangle sourceRect;

        public GameTex(Texture2D tex, Color color, Rectangle sourceRect)
        {
            this.tex = tex;
            this.color = color;
            this.color2 = color;
            this.sourceRect = sourceRect;
        }
        public GameTex(Texture2D tex, Color color, Color color2, Rectangle sourceRect)
        {
            this.tex = tex;
            this.color = color;
            this.color2 = color2;
            this.sourceRect = sourceRect;
        }

        public void Draw(SpriteBatch spriteBatch, ref Rectangle destRect, float lerpValue = 0)
        {
            spriteBatch.Draw(tex, destRect,sourceRect, Color.Lerp(color, color2,lerpValue));
        }

        public void Draw(SpriteBatch spriteBatch, ref Rectangle destRect)
        {
            spriteBatch.Draw(tex, destRect, sourceRect, color);
        }
    }
}
