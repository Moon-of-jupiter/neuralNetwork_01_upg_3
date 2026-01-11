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
        protected HeadSimulatorManager headSimulatorManager;
        protected SnakeRenderer sr;
        public GameManager(GraphicsDevice gd)
        {
            var a = new EvoluionSimData
            {
                neuralNetwork_Height = 6,
                neuralNetwork_Width = 4,
                population = 30,

                mapSize = new Point(50,50),
                
            };

            headSimulatorManager = new HeadSimulatorManager(a);

            headSimulatorManager._evolutionManager.SetEvolutionManagers
                (
                new Selector01(0.7f, 1f, 31),
                new Crossover01(),
                new Mutator01(0.3f, 0.5f, 30)

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
            sr.Render(_spriteBatch,headSimulatorManager._simulationManager.simulators[0]);



            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap);

            _spriteBatch.Draw(sr.renderTarget, new Rectangle(0, 0, 400, 400), Color.White);

            _spriteBatch.End();
        }

    }
}
