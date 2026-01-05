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
        public Rectangle sourceRect;

        public GameTex(Texture2D tex, Color color, Rectangle sourceRect)
        {
            this.tex = tex;
            this.color = color;
            this.sourceRect = sourceRect;
        }

        public void Draw(SpriteBatch spriteBatch, ref Rectangle destRect)
        {
            spriteBatch.Draw(tex, destRect,sourceRect, color);
        }
    }
}
