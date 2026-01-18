using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using neuralNetwork_01_upg_3.Helpers;
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

        private bool secondFrame = true;

        private ButtonHelper pauseBtn = new();
        private ButtonHelper toggleBorderlessBtn = new();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
            //Window.IsBorderless = true;
            
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            pauseBtn.OnPress += OnPauseBtn;
            toggleBorderlessBtn.OnPress += OnBorderlessBtn;

        }

        
        

        protected override void LoadContent()
        {
            AssetManagerSingleton.TextureManager.Initialize(Content, "Pixel");
            AssetManagerSingleton.FontManager.Initialize(Content, "font1");
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            AssetManagerSingleton.TextureManager.LoadTexture("placeholder", "Pixel");
            AssetManagerSingleton.TextureManager.LoadTexture("circle", "circleSprite");
            AssetManagerSingleton.FontManager.LoadTexture("placeholder", "font1");
            
            // TODO: use this.Content to load your game content here
        }

        protected void OnFirstFrame()
        {
            test = new TestClass(_graphics.GraphicsDevice);
            gm = new GameManager(_graphics.GraphicsDevice);
            firstFrame = false;
        }

        protected bool isOn = false;

        private void OnPauseBtn(bool btnState)
        {
            if (!btnState) return;

            isOn = !isOn;
        }

        private void OnBorderlessBtn(bool btnState)
        {
            if (!btnState) return;

            Window.IsBorderless = !Window.IsBorderless;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Delete))
                Exit();

            if (firstFrame)
            {
                OnFirstFrame();
            }


            pauseBtn.UpdatePressed(Keyboard.GetState().IsKeyDown(Keys.Space));
            toggleBorderlessBtn.UpdatePressed(Keyboard.GetState().IsKeyDown(Keys.Tab));


            if (!isOn)
            {
                base.Update(gameTime);
                return;

            }
            

            



            gm.Update();

            //Window.Title = gm.GameText;
            


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
