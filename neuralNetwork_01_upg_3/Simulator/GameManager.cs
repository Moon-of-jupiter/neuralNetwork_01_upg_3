using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using neuralNetwork_01_upg_3.Helpers;
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
        public string GameText = "no data";
        
        public int AliveSnakes => headSimulatorManager._simulationManager.AlivePopulation;

        public HeadSimulatorManager headSimulatorManager;
        protected SnakeRenderer sr;


        private ButtonHelper SwitchStateButton = new ButtonHelper();
        public int RenderingState = 0;
        private bool switchStateKeyPressed = false;


        private Rectangle displayRect = new Rectangle(0, 0, 400, 400);

        private Rectangle graphRect = new Rectangle(450, 200, 200, 200);
        private Rectangle graphRect2 = new Rectangle(0, 440, 800, 30);


        private Vector2 textPos = new Vector2(450,30);

        private GraphRenderer graphRenderer;
        private GraphRenderer graphRenderer2;


        private Color[] colorList;

        public GameManager(GraphicsDevice gd)
        {
            SwitchStateButton.OnPress += OnChangeRenderState;

            var a = new EvoluionSimData
            {
                neuralNetwork_Height = 5, // 5
                neuralNetwork_Width = 2, // 2 
                population = 5000,

                mapSize = new Point(25,25),
                
            };

            Random rd = new Random();

            headSimulatorManager = new HeadSimulatorManager(a,rd.Next());

            headSimulatorManager._evolutionManager.SetEvolutionManagers
                (
                new Selector01(0.7f, 1f, rd.Next()),
                new Crossover01(rd.Next()),
                new Mutator01(0.005f,0.01f, 0.01f, rd.Next())

                );

            colorList = new Color[]
            {
                new Color(10,10,10),
                Color.Red,
                Color.White,
                Color.MediumAquamarine,
                

            };

            sr = new SnakeRenderer(SnakeRenderer.CreuateSnakeRenderData(gd, 1, AssetManagerSingleton.TextureManager.GetAsset("snakeMapSprites"), colorList));
            
            sr.SetSnakeGame(headSimulatorManager._simulationManager.simulators[0]);


            graphRenderer = new GraphRenderer(gd, new GraphRenderer_Data()
            {
                Bounds = new Point(100, 100),
                lineThickness = 1,
                color = colorList[3],
                background_color = colorList[0],
                tex = AssetManagerSingleton.TextureManager.GetAsset("graph"),
                connection_tex = AssetManagerSingleton.TextureManager.GetAsset("circle"),
            }, headSimulatorManager.Scores);


            graphRenderer2 = new GraphRenderer(gd, new GraphRenderer_Data()
            {
                Bounds = new Point(800, 30),
                lineThickness = 1,
                color = colorList[3],
                background_color = colorList[0],
                tex = AssetManagerSingleton.TextureManager.GetAsset("graph"),
                connection_tex = AssetManagerSingleton.TextureManager.GetAsset("circle"),
            }, headSimulatorManager.Scores);

            graphRenderer2.UpdateRT();
            graphRenderer.UpdateRT();


            var v = new List<float>();
            graphRenderer2.values = v;

            v.Add(10);
            v.Add(10);
            v.Add(10);

        }


        public void Update()
        {
            headSimulatorManager.Update();

            
            BestScore = headSimulatorManager.BestScore;
            UpdateChangeRenderState();

            //ChangeRenderState();

            GameText = $" View: {RenderingState}, Generation: {headSimulatorManager.Generation}\n Target Snake {CurrentTarget} / {AliveSnakes}\n Score: {((int)(CurrentScore * 10)) / 10f}, Best of Current {((int)(BestScore * 10)) / 10f}\n Last Gen Score: {(int)(headSimulatorManager.HighestScoreLastRound * 10) / 10f}, High Score: {(int)(headSimulatorManager.HighestScoreEver * 10) / 10f}\n Frame: {headSimulatorManager.GameFrame}\n Geneome Length; {headSimulatorManager._evolutionManager.population[0].genome.Length}";
        }

        

        
        private void OnChangeRenderState(bool btnState)
        {
            if (!btnState) return;

            RenderingState++;
            RenderingState %= 3;
        }

        private void UpdateChangeRenderState()
        {
            SwitchStateButton.UpdatePressed(Keyboard.GetState().IsKeyDown(Keys.Enter));
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

            float r = MathF.Cos(entety.genome[entety.genome.Length - 1]);
            float g = MathF.Cos(entety.genome[entety.genome.Length - 2]);
            float b = MathF.Cos(entety.genome[entety.genome.Length - 3]);
            sr.renderData.tileType_GameTextures[2].color = new Color(r, g, b, 1);

            sr.Render(_spriteBatch, headSimulatorManager._simulationManager.simulators[snakeSimId]);

            if (currentTarget != snakeSimId)
            {
                graphRenderer2.values = headSimulatorManager._evolutionManager.population[snakeSimId].genome.ToList();
            }
                

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



            graphRenderer.Render(_spriteBatch);
            graphRenderer2.Render(_spriteBatch);


            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap);

            _spriteBatch.Draw(sr.renderTarget, displayRect, Color.White);

            _spriteBatch.Draw(AssetManagerSingleton.TextureManager.GetAsset("a"), graphRect, colorList[0]);
            _spriteBatch.Draw(graphRenderer.renderTarget, graphRect, Color.White);
            _spriteBatch.Draw(graphRenderer2.renderTarget, graphRect2, Color.White);

            _spriteBatch.DrawString(AssetManagerSingleton.FontManager.GetAsset("a"), GameText, textPos, colorList[3]);

            _spriteBatch.End();
        }

    }
}
