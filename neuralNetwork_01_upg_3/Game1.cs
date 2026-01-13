using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using neuralNetwork_01_upg_3.Simulator;
using neuralNetwork_01_upg_3.Simulator.NeuralNet;
using neuralNetwork_01_upg_3.Simulator.NeuralNet.ActivationFunctions;
using neuralNetwork_01_upg_3.Textures;
using SharpDX;
using System;
using System.Diagnostics;
using System.Threading;
//using System.Windows.Forms;

namespace neuralNetwork_01_upg_3
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private TestClass test;
        private GameManager gm;

        private bool firstFrame = true;

        
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();



        }

        
        

        protected override void LoadContent()
        {
            TextureManager.Initialize(Content, "Pixel");
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            TextureManager.LoadTexture("placeholder", "Pixel");
            // TODO: use this.Content to load your game content here
        }

        protected void OnFirstFrame()
        {
            test = new TestClass(_graphics.GraphicsDevice);
            gm = new GameManager(_graphics.GraphicsDevice);
            firstFrame = false;
        }

        protected bool isOn = false;
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Delete))
                Exit();

            if (firstFrame)
            {
                OnFirstFrame();
            }

            if (!isOn)
            {
                if (!Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    base.Update(gameTime);
                    return;
                }
                

                isOn = true;
            }
            

            



            gm.Update();

            Window.Title = $" view: {gm.RenderingState}, Generation: {gm.headSimulatorManager.Generation}, Target Snake {gm.CurrentTarget} / {gm.AliveSnakes}, Score: {((int)(gm.CurrentScore * 10)) / 10f}, Best of Current {((int)(gm.BestScore * 10)) / 10f}";


            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.CornflowerBlue);

            gm.Render(_spriteBatch);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
