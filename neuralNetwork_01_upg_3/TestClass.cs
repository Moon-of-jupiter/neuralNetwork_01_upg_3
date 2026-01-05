using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using neuralNetwork_01_upg_3.RNG;
using neuralNetwork_01_upg_3.Simulator.Game.Snake;
using neuralNetwork_01_upg_3.Textures;
using System;
using System.Collections.Generic;
//using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neuralNetwork_01_upg_3
{
    public class TestClass
    {
        protected GraphicsDevice gd;

        protected SnakeSimulator snakeSimulator;
        protected SnakeRenderer snakeRenderer;

        public TestClass(GraphicsDevice gd)
        {
            var t = new TestSnakeControler();
            Random random = new Random();

            t.seed = random.Next();

            snakeSimulator = new SnakeSimulator(t, new SnakeMapData()
            {
                size = new Point(20, 20),
                seed = random.Next()
            });

            this.gd = gd;

            var colors = new Color[]
            {
                new Color(10,10,10),
                Color.Red,
                Color.White,
            };

            var rd = SnakeRenderer.CreuateSnakeRenderData(gd, 1, TextureManager.GetTexture("SnakeSprites"), colors);

            snakeRenderer = new SnakeRenderer(rd);
            snakeRenderer.SetSnakeGame(snakeSimulator);
        }
        private Point start = new Point(0,0);
        private Point direction = new Point(1, 1);
        public void Update()
        {
            snakeSimulator.Update();
            bool hit = snakeSimulator.Raycast(ref start, ref direction, out int length, SnakeSimulator.MapElementType.snake);
            if (hit)
            {
                int i = 0;
            }

        }

        public void Render(SpriteBatch sb)
        {
            snakeRenderer.Render(sb);

            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap);

            sb.Draw(snakeRenderer.renderTarget, new Rectangle(0,0,400,400),Color.White);

            sb.End();
        }

    }

    public class TestSnakeControler: ISnakeControler
    {
        public Point[] directions = new Point[]
        {
            new Point(1,0),
            new Point(0,1),
            new Point(-1,0),
            new Point(0,-1),
        };


        private Point lastMove;
        private Point rev_lastMove;
        public int seed;

        private float counter = 0;
        private float speed = (float)Math.PI / 10f;
        public Point Control(SnakeSimulator snake)
        {

            var rng = CustomRandom.ShiftRandomXOr(CustomRandom.ShiftRandomXOr((int)(counter += speed) + seed));

            var direction = directions[Math.Abs(rng) % directions.Length];

            if (direction == rev_lastMove)
                direction = lastMove;

            lastMove = direction;   
            rev_lastMove.X = -direction.X;
            rev_lastMove.Y = -direction.Y;

            return direction;
        }
    }
}
