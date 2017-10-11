using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SGS.Components.Cameras;
using SGS.Components.Input;
using SGS.Components.Scenes;
using SGS.Components.UI;
using System;

namespace SGS.Components
{
    /// <summary>
    /// Componente responsável pelo gerenciament do ciclo de vida do jogo em curso
    /// </summary>
    public static class GameManager
    {
        private const int DEFAULT_FIRST_SCENE = 0;
        private static readonly PauseStateGameController DEFAULT_PAUSE_CONTROLLER = new PauseStateGameController();

        public enum GameStates
        {
            Unloaded,
            Ready,
            Loading,
            Running,
            Paused
        }

        private static MyGame game;
        private static GameStates state;
        private static GameScene[] scenes;
        private static Int32 currentSceneIndex;
        private static SpriteBatch spriteBatch;
        private static SemiTransparentDarkPanel PAUSE_PANEL;

        public static DefaultGameCamera MainCamera { get; private set; }

        public static GameInputManager Input { get; private set; }

        public static SoundManager Sound { get; private set; }

        public static ContentManager Content
        {
            get
            {
                return game.Content;
            }
        }

        public static GameServiceContainer Services
        {
            get
            {
                return game.Services;
            }
        }

        public static GraphicsDevice GraphicsDevice
        {
            get
            {
                return game.GraphicsDevice;
            }
        }

        /// <summary>
        /// Estado atual do jogo
        /// </summary>
        public static GameStates State
        {
            get { return state; }
        }

        /// <summary>
        /// Scene atualmente em execução
        /// </summary>
        public static GameScene CurrentScene
        {
            get { return scenes[currentSceneIndex]; }
        }

        /// <summary>
        /// Carrega conteúdo do tipo não gráfico
        /// </summary>
        public static void Initialize(MyGame g)
        {
            game = g;
        }

        /// <summary>
        /// Carrega conteúdo inicial, comum a todas as cenas de jogo, e registra serviços
        /// úteis capaz de serem utilizados pelos GameObjects
        /// </summary>
        /// <param name="content"></param>
        /// <param name="services"></param>
        public static void LoadContent()
        {
            if (state != GameStates.Unloaded)
                return;

            spriteBatch = new SpriteBatch(GraphicsDevice);

            MainCamera = new DefaultGameCamera(game.GraphicsDevice);
            MainCamera.Initialize();

            Input = new GameInputManager(game, NoActionGameController.Instance);
            Input.Initialize();

            Sound = new SoundManager();

            scenes = new GameScene[]
            {
                new TitleScreenScene(),
                new MapBasedGameScene(5, "maps/lab_map_005", Color.Black),
                new MapBasedGameScene(4, "maps/lab_map_004", Color.Black),
                new CreditsScene()
            };

            PAUSE_PANEL = new SemiTransparentDarkPanel();
            PAUSE_PANEL.LoadContent();

            currentSceneIndex = DEFAULT_FIRST_SCENE;
            state = GameStates.Ready;

        }

        public static void Update(GameTime t)
        {
            Input.Update(t);

            if (State == GameStates.Running)
                CurrentScene.Update(t);
        }

        public static void Draw()
        {
            GraphicsDevice.Clear(CurrentScene.BackgroundColor);
            CurrentScene.Draw(spriteBatch);

            if (state == GameStates.Paused)
            {
                ShowPauseScreen();
            }
        }

        /// <summary>
        /// Inicia o processo de carregamento e inicialização da GameScene atual
        /// </summary>
        public static void Play()
        {
            if (state != GameStates.Ready && state != GameStates.Running)
                return;

            state = GameStates.Loading;


            CurrentScene.LoadContent();
            CurrentScene.Initialize();

            Input.MainGameController = CurrentScene.GameController;
            state = GameStates.Running;
        }

        /// <summary>
        /// Solicita execução da próxima GameScene
        /// </summary>
        public static void PlayNextScene()
        {
            if (state != GameStates.Running)
                return;

            var nextSceneIndex = currentSceneIndex + 1;

            if (nextSceneIndex < scenes.Length)
            {
                CurrentScene.Unload();
                currentSceneIndex = nextSceneIndex;
                Play();
            }
            else
            {
                Reset();
            }
        }

        /// <summary>
        /// Reinicia a execução do game manager
        /// </summary>
        public static void Reset()
        {
            CurrentScene.Unload();
            currentSceneIndex = DEFAULT_FIRST_SCENE;
            state = GameStates.Ready;
            Play();
        }

        /// <summary>
        /// Interrompe a execução da GameScene atual
        /// </summary>
        public static void Pause()
        {
            if (state != GameStates.Running)
                return;

            CurrentScene.Pause();
            Input.MainGameController = DEFAULT_PAUSE_CONTROLLER;

            state = GameStates.Paused;
        }

        /// <summary>
        /// Retoma a execução da GameScene atual
        /// </summary>
        public static void Resume()
        {
            if (state != GameStates.Paused)
                return;

            CurrentScene.Resume();
            Input.MainGameController = CurrentScene.GameController;

            state = GameStates.Running;
        }

        /// <summary>
        /// Retoma ou pausa o jogo conforme estado atual
        /// </summary>
        public static void TogglePause()
        {
            if (State == GameStates.Paused)
                Resume();
            else
                Pause();
        }

        /// <summary>
        /// Encerra a execução do jogo
        /// </summary>
        public static void End()
        {
            game.Exit();
        }

        /// <summary>
        /// Solicita encerramento da GameScene atual
        /// </summary>
        public static void CurrentSceneCompleted()
        {
            CurrentScene.Complete();
        }

        /// <summary>
        /// Informa que o personagem morreu (TODO: verificar se essa é a melhor abordagem para propagar eventos importantes dentro do jogo)
        /// </summary>
        public static void PlayerDied()
        {
            MainCamera.LookAt(CurrentScene.Player.Position);
        }

        /// <summary>
        /// Mostra um painel escuro, semitransparente indicando que jogo está em pause
        /// </summary>
        private static void ShowPauseScreen()
        {
            spriteBatch.Begin();
            PAUSE_PANEL.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
