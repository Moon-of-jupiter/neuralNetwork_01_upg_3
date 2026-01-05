using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
//using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neuralNetwork_01_upg_3.Simulator.Game.Snake
{
    public class SnakeRenderer
    {
        protected SnakeRendererData renderData;


        protected SnakeSimulator snakeSimulator;

        protected Rectangle bounds;
        public RenderTarget2D renderTarget { get; protected set; }


        public SnakeRenderer(SnakeRendererData rendererData)
        {
            renderData = rendererData;
        }

        

        public static SnakeRendererData CreuateSnakeRenderData(GraphicsDevice gd, int tileRes, Texture2D tileTex, Color[] typeColors)
        {
            GameTex[] gameTextures = new GameTex[typeColors.Length];

            for (int i = 0; i < gameTextures.Length; i++)
            {
                gameTextures[i] = new GameTex(tileTex, typeColors[i], tileTex.Bounds);
            }

            return new SnakeRendererData(gd,tileRes, gameTextures);

        }

        public void SetSnakeGame(SnakeSimulator game)
        {
            this.snakeSimulator = game;
            UpdateBounds();
        }

        public void UpdateBounds()
        {
            if(renderTarget != null)
                renderTarget.Dispose();

            bounds.Width = snakeSimulator.map.GetLength(0) * renderData.tileRes;
            bounds.Height = snakeSimulator.map.GetLength(1) * renderData.tileRes;

            renderTarget = new RenderTarget2D(renderData.gd, bounds.Width, bounds.Height);

        }

        public void Render(SpriteBatch sb)
        {
            renderData.gd.SetRenderTarget(renderTarget);
            
            renderData.gd.Clear(Color.Black);

            sb.Begin();

            Point pos = new Point(0,0);

            for(int x = 0; x < snakeSimulator.map.GetLength(0); x++)
            {
                pos.X = x;
                for (int y = 0; y < snakeSimulator.map.GetLength(1); y++)
                {
                    pos.Y = y;

                    renderData.DrawTile(sb, ref pos, (int)snakeSimulator.GetElementAtPos(ref pos));
                }
            }

            sb.End();

            renderData.gd.SetRenderTarget(null);

        }


    }

    public class SnakeRendererData
    {
        public GraphicsDevice gd;

        public int tileRes;
        public GameTex[] tileType_GameTextures;

        private Rectangle rect;

        public SnakeRendererData(GraphicsDevice gd, int tileRes, GameTex[] tileType_GameTextures)
        {
            this.gd = gd;
            this.tileType_GameTextures = tileType_GameTextures;
            SetTileRes(tileRes);
        }

        public void SetTileRes(int tileRes)
        {
            this.tileRes = tileRes;
            rect.Width = tileRes;
            rect.Height = tileRes;
        }

        public void DrawTile(SpriteBatch sb, ref Point tilePos, int tileType)
        {
            if (tileType > tileType_GameTextures.Length || tileType < 0) return;
            rect.X = tilePos.X * tileRes;
            rect.Y = tilePos.Y * tileRes;

            tileType_GameTextures[tileType].Draw(sb, ref rect);

        }
    }

    
    
}
