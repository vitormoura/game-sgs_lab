using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using SGS.Components.Debug;
using SGS.Components.Players;
using SGS.Components.Input;
using SGS.Components.World;

namespace SGS.Components.Scenes
{
    public abstract class GameScene : DrawableGameObject
    {
        private GameSceneLayer[] layers;
                
        private const int DEFAULT_QTD_LAYERS = 7;
        private const int DEFAULT_QTD_WORLD_LAYERS = 4;
                
        public const int LAYER_WORLD_0 = 0;
        public const int LAYER_WORLD_1 = 1;
        public const int LAYER_WORLD_2 = 2;
        public const int LAYER_WORLD_DEBUG = 3;
        public const int LAYER_HUD_1 = 4;
        public const int LAYER_HUD_2 = 5;
        public const int LAYER_HUD_DEBUG = 6;

        public GameSceneRunningSession Session
        {
            get;
            private set;
        }

        public IGameInputController GameController
        {
            get;
            protected set;
        }

        public Player Player
        {
            get;
            protected set;
        }

        public GameWorld World
        {
            get;
            protected set;
        }

        public Color BackgroundColor { get; set; }

        public Boolean VisualDebugMode
        {
            get
            {
                return this.layers[LAYER_WORLD_DEBUG].Enabled;
            }

            set
            {
                if (value)
                {
                    this.layers[LAYER_HUD_DEBUG].Enable();
                    this.layers[LAYER_WORLD_DEBUG].Enable();
                }
                else
                {
                    this.layers[LAYER_HUD_DEBUG].Disable();
                    this.layers[LAYER_WORLD_DEBUG].Disable();
                }
            }
        }

        public GameScene()
            : base()
        {
            this.layers = new GameSceneLayer[DEFAULT_QTD_LAYERS];

            for (int i = 0; i < DEFAULT_QTD_LAYERS; i++)
            {
                this.layers[i] = new GameSceneLayer();
            }
                                                
            this.GameController = NoActionGameController.Instance;
        }

        public override void LoadContent()
        {
            this.AddChild(GameManager.MainCamera);

            for (int i = 0; i < DEFAULT_QTD_LAYERS; i++)
            {
                base.AddChild(this.layers[i]);
            }
                        

#if DEBUG
            var debugPanel = new VisualDebugPanel(Color.Red);
            var screenMsgLog = new ScreenMessageLog(Color.White);
            var fps = new FPSCounter(screenMsgLog);
            var cameraVisualDebug = new CameraVisualDebug(GameManager.MainCamera);

            GameManager.Services.RemoveService(typeof(IVisualDebugService));
            GameManager.Services.RemoveService(typeof(IMessageDebugService));
            GameManager.Services.AddService<IVisualDebugService>(debugPanel);
            GameManager.Services.AddService<IMessageDebugService>(screenMsgLog);

            this.AddChild(LAYER_WORLD_DEBUG, debugPanel);
            this.AddChild(LAYER_WORLD_DEBUG, cameraVisualDebug);
            this.AddChild(LAYER_HUD_DEBUG, screenMsgLog);
            this.AddChild(LAYER_HUD_DEBUG, fps);
#endif

            base.LoadContent();
        }

        public override void Initialize()
        {
            base.Initialize();

            Session = new GameSceneRunningSession();
            Session.Begin();

            this.VisualDebugMode = false;
        }
        
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (this.Enabled)
            {
                var viewMatrix = GameManager.MainCamera.GetViewMatrix();

                for (int i = 0; i < this.layers.Length; i++)
                {
                    this.layers[i].ViewTransform = i < DEFAULT_QTD_WORLD_LAYERS ? viewMatrix : Matrix.Identity;
                }
            }
        }

        /// <summary>
        /// Encerra sessão da gamescene
        /// </summary>
        /// <param name="session"></param>
        public virtual void Complete()
        {
            Session.Finish();
        }

        /// <summary>
        /// Interrompe execução da gamescene
        /// </summary>
        public void Pause()
        {
            Enabled = false;
            Session.Pause();
        }

        /// <summary>
        /// Retoma execução de uma gamescene em pausa
        /// </summary>
        public void Resume()
        {
            Enabled = true;
            Session.Resume();
        }

        public override void Unload()
        {
            base.Unload();
        }

        /// <summary>
        /// Adiciona componente a primeira camada do mundo
        /// </summary>
        /// <param name="c"></param>
        public override void AddChild(GameObject c)
        {
            this.AddChild(LAYER_WORLD_0, c);
        }

        protected void SetLayerEnabledAndVisible(Int32 index, Boolean enabled)
        {
            this.SetLayerEnabled(index, enabled);
            this.SetLayerVisible(index, enabled);
        }

        /// <summary>
        /// Redefine se camadas estão ou não habilitadas
        /// </summary>
        /// <param name="index"></param>
        /// <param name="enabled"></param>
        protected void SetLayerEnabled(Int32 index, Boolean enabled)
        {
            this.layers[index].Enabled = enabled;
            
        }

        /// <summary>
        /// Redefine se camadas estão ou não visíveis
        /// </summary>
        /// <param name="index"></param>
        /// <param name="visible"></param>
        protected void SetLayerVisible(Int32 index, Boolean visible)
        {
            this.layers[index].Visible = visible;
        }

        /// <summary>
        /// Habilita ou não camadas do mundo de jogo
        /// </summary>
        /// <param name="enabled"></param>
        protected void SetWorldLayersEnabled(Boolean enabled)
        {
            for (int i = 0; i < DEFAULT_QTD_WORLD_LAYERS; i++)
            {
                SetLayerEnabled(i, enabled);
            }
        }

        protected void SetWorldLayersVisible(Boolean visible)
        {
            for (int i = 0; i < DEFAULT_QTD_WORLD_LAYERS; i++)
            {
                SetLayerVisible(i, visible);
            }
        }

        /// <summary>
        /// Desabilita todas as camadas
        /// </summary>
        protected void DisableAndHideAllLayers()
        {
            for (int i = 0; i < this.layers.Length; i++)
            {
                this.SetLayerEnabled(i, false);
                this.SetLayerVisible(i, false);
            }
        }
                                        
        /// <summary>
        /// Adiciona componente a camada informada
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="c"></param>
        protected void AddChild(Int32 layer, GameObject c)
        {
            System.Diagnostics.Debug.Assert(layer >= 0 && layer < this.layers.Length);
                        
            this.layers[layer].AddChild(c);
        }

        /// <summary>
        /// Adiciona vários componentes a camada informada
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="components"></param>
        protected void AddChildren(Int32 layer, IEnumerable<GameObject> components)
        {
            foreach (var c in components)
                this.AddChild(layer, c);
        }

        /// <summary>
        /// Adiciona vários componentes a uma camada utilizando a sintaxe 'params'
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="components"></param>
        protected void AddChildren(Int32 layer, params GameObject[] components)
        {
            this.AddChildren(layer, (IEnumerable<GameObject>)components);
        }
    }
}
