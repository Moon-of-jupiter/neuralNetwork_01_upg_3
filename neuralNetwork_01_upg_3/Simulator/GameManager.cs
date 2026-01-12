using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        public int AliveSnakes => headSimulatorManager._simulationManager.AlivePopulation;

        public HeadSimulatorManager headSimulatorManager;
        protected SnakeRenderer sr;
        public GameManager(GraphicsDevice gd)
        {
            var a = new EvoluionSimData
            {
                neuralNetwork_Height = 10,
                neuralNetwork_Width = 3,
                population = 1000,

                mapSize = new Point(35,35),
                
            };

            Random rd = new Random();

            headSimulatorManager = new HeadSimulatorManager(a);

            headSimulatorManager._evolutionManager.SetEvolutionManagers
                (
                new Selector01(0.8f, 1f, rd.Next()),
                new Crossover01(),
                new Mutator01(0.1f,0.3f, 0.01f, rd.Next())

                );

            var colors = new Color[]
            {
                new Color(10,10,10),
                Color.Red,
                Color.White,
            };

            sr = new SnakeRenderer(SnakeRenderer.CreuateSnakeRenderData(gd, 1, TextureManager.GetTexture("snakeMapSprites"), colors));
            
            sr.SetSnakeGame(headSimulatorManager._simulationManager.simulators[0]);
        }


        public void Update()
        {
            headSimulatorManager.Update();
        }

        public void Render(SpriteBatch _spriteBatch)
        {
            
            for(int i = 0; i < headSimulatorManager._simulationManager.simulators.Length; i++)
            {
                if (headSimulatorManager._simulationManager.simulators[i].gameOver)
                    continue;
                var entety = headSimulatorManager._evolutionManager.population[i];

                float r = MathF.Sin(entety.genome[entety.genome.Length - 1]);
                float g = MathF.Sin(entety.genome[entety.genome.Length - 2]);
                float b = MathF.Sin(entety.genome[entety.genome.Length - 3]);
                sr.renderData.tileType_GameTextures[2].color = new Color(r,g,b,1);

                sr.Render(_spriteBatch, headSimulatorManager._simulationManager.simulators[i]);
                CurrentTarget = i;
                CurrentScore = headSimulatorManager._simulationManager.simulators[i].score;
                break;
            }


            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap);

            _spriteBatch.Draw(sr.renderTarget, new Rectangle(0, 0, 400, 400), Color.White);

            _spriteBatch.End();
        }

    }
}
