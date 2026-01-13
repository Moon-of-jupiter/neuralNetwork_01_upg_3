using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using neuralNetwork_01_upg_3.Simulator.Evolution.SubProcessies;
using neuralNetwork_01_upg_3.Simulator.Game.Snake;
using neuralNetwork_01_upg_3.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neuralNetwork_01_upg_3.Simulator
{
    public class GameManager
    {
        public int Generation => headSimulatorManager.Generation;
        public int CurrentTarget;
        public float CurrentScore;
        public float BestScore;
        
        public int AliveSnakes => headSimulatorManager._simulationManager.AlivePopulation;

        public HeadSimulatorManager headSimulatorManager;
        protected SnakeRenderer sr;

        public int RenderingState = 0;
        private bool switchStateKeyPressed = false;


        private Rectangle displayRect = new Rectangle(0, 0, 400, 400);

        private Rectangle graphRect = new Rectangle(450, 200, 200, 200);

        private GraphRenderer graphRenderer;

        public GameManager(GraphicsDevice gd)
        {
            var a = new EvoluionSimData
            {
                neuralNetwork_Height = 10,
                neuralNetwork_Width = 3,
                population = 10000,

                mapSize = new Point(25,25),
                
            };

            Random rd = new Random();

            headSimulatorManager = new HeadSimulatorManager(a,rd.Next());

            headSimulatorManager._evolutionManager.SetEvolutionManagers
                (
                new Selector01(0.7f, 1f, rd.Next()),
                new Crossover01(rd.Next()),
                new Mutator01(0.05f,0.1f, 0.01f, rd.Next())

                );

            var colors = new Color[]
            {
                new Color(10,10,10),
                Color.Red,
                Color.White,
            };

            sr = new SnakeRenderer(SnakeRenderer.CreuateSnakeRenderData(gd, 1, TextureManager.GetTexture("snakeMapSprites"), colors));
            
            sr.SetSnakeGame(headSimulatorManager._simulationManager.simulators[0]);


            graphRenderer = new GraphRenderer(new GraphRenderer_Data()
            {
                Bounds = graphRect,
                lineThickness = 1,
                color = Color.Aquamarine,
                tex = TextureManager.GetTexture("graphTex")
            }, headSimulatorManager.Scores);

        }


        public void Update()
        {
            headSimulatorManager.Update();

            
            BestScore = headSimulatorManager.BestScore;
            ChangeRenderState();
        }

        

        

        private void ChangeRenderState()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                if (!switchStateKeyPressed)
                {
                    RenderingState++;
                    RenderingState %= 3;
                }

                switchStateKeyPressed = true;
            }
            else
            {
                switchStateKeyPressed = false;
            }
                
        }
        
        
        private void RenderSnake(SpriteBatch _spriteBatch, int snakeSimId)
        {
            var entety = headSimulatorManager._evolutionManager.population[snakeSimId];

            float r = MathF.Sin(entety.genome[entety.genome.Length - 1]);
            float g = MathF.Sin(entety.genome[entety.genome.Length - 2]);
            float b = MathF.Sin(entety.genome[entety.genome.Length - 3]);
            sr.renderData.tileType_GameTextures[2].color = new Color(r, g, b, 1);

            sr.Render(_spriteBatch, headSimulatorManager._simulationManager.simulators[snakeSimId]);
            CurrentTarget = snakeSimId;
            CurrentScore = headSimulatorManager._simulationManager.simulators[snakeSimId].score;
        }


        
        private void RenderSnakesUntillDeath(SpriteBatch _spriteBatch)
        {
            for (int i = 0; i < headSimulatorManager._simulationManager.simulators.Length; i++)
            {
                if (headSimulatorManager._simulationManager.simulators[i].gameOver)
                    continue;

                RenderSnake(_spriteBatch, i);

                break;
            }
        }

        private void RenderBestUntillDeath(SpriteBatch _spriteBatch)
        {
            for (int i = 0; i < headSimulatorManager._simulationManager.simulators.Length; i++)
            {
                int accual = (i + headSimulatorManager.BestPhenotype) % headSimulatorManager._simulationManager.simulators.Length;

                if (headSimulatorManager._simulationManager.simulators[accual].gameOver)
                    continue;

                RenderSnake(_spriteBatch, accual);

                break;
            }
        }

        private float currentTarget;
        
        private void RennderSnakesOneEachFrame(SpriteBatch _spriteBatch, float speedOfChange, bool skip_dead)
        {

            currentTarget += speedOfChange;
            currentTarget %= headSimulatorManager._simulationManager.simulators.Length;

            if (skip_dead)
            {
                while (headSimulatorManager._simulationManager.simulators[(int)currentTarget].gameOver)
                {
                    currentTarget += 1;
                    currentTarget %= headSimulatorManager._simulationManager.simulators.Length;
                }
            }

            RenderSnake(_spriteBatch, (int)currentTarget);
        }

        public void Render(SpriteBatch _spriteBatch)
        {

            switch (RenderingState)
            {
                case 1:
                    RenderSnakesUntillDeath(_spriteBatch);
                    break;

                case 2:
                    RennderSnakesOneEachFrame(_spriteBatch, 0.25f, true);
                    break;

                default:
                    RenderBestUntillDeath(_spriteBatch);
                    break;
            }
            




            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap);

            _spriteBatch.Draw(sr.renderTarget, displayRect, Color.White);

            _spriteBatch.Draw(TextureManager.GetTexture("a"), graphRenderer.data.Bounds, new Color(30, 30, 30));
            graphRenderer.Render(_spriteBatch);
            

            _spriteBatch.End();
        }

    }
}
