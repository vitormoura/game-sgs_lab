using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SGS.Components;
using System;

namespace SGS
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MyGame : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch defaultSpriteBatch;
        private Color defaultCanvasColor;

        private bool WindowSizeIsBeingChanged = false;
        private Texture2D texture;
        private Effect effect;
        private RenderTarget2D renderTarget;

        public MyGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 780;
            graphics.PreferredBackBufferWidth = 1024;
            Window.AllowUserResizing = true;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            GameManager.Initialize(this);

            Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);

            renderTarget = new RenderTarget2D(
                GraphicsDevice,
                GraphicsDevice.PresentationParameters.BackBufferWidth,
                GraphicsDevice.PresentationParameters.BackBufferHeight,
                false,
                GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            this.defaultSpriteBatch = new SpriteBatch(GraphicsDevice);

            GameManager.LoadContent();
            GameManager.Play();

            texture = GameManager.Content.Load<Texture2D>("sprites/bomb"); //DUMMY SPRITE
            effect = GameManager.Content.Load<Effect>("shaders/ClosingVignettePixelShader");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            GameManager.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            /* Filtro Sépia
            GraphicsDevice.SetRenderTarget(renderTarget);

            GameManager.Draw();
            base.Draw(gameTime);
                        
            GraphicsDevice.SetRenderTarget(null);

            defaultSpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            effect.CurrentTechnique.Passes[2].Apply();
            defaultSpriteBatch.Draw(renderTarget, renderTarget.Bounds, Color.White);
            defaultSpriteBatch.End();
            //*/

            ///* Sem filtro especial
            GameManager.Draw();
            base.Draw(gameTime);
            //*/
        }

        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            WindowSizeIsBeingChanged = !WindowSizeIsBeingChanged;

            if (WindowSizeIsBeingChanged)
            {
                graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
                graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
                graphics.ApplyChanges();
            }
        }
    }
}
