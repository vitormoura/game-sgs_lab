using Microsoft.Xna.Framework;
using SGS.Components.Input;
using SGS.Components.Players;
using SGS.Components.Tasks;
using SGS.Components.UI;
using SGS.Components.World;
using System;

namespace SGS.Components.Scenes
{
    public class MapBasedGameScene : GameScene
    {
        private const int LAYER_OPENING_CONCLUSION = GameScene.LAYER_HUD_2;
        private const int LAYER_CLOSING_VIGNETTE = GameScene.LAYER_HUD_1;

        private Wait startPanelClose;
        private SceneStartPanel startPanel;
        private SceneCompletationPanel completation;
        private String gameWorldMap;
        private ClosingVignette closingVignette;

        public object Instance { get; private set; }

        public MapBasedGameScene(int sceneIndex, String gameWorldMap)
            : this(sceneIndex, gameWorldMap, new Color(0xED, 0xF9, 0xFF))
        {
        }

        public MapBasedGameScene(Int32 sceneIndex, String gameWorldMap, Color backgroundColor)
            : base()
        {
            this.BackgroundColor = backgroundColor;
            this.gameWorldMap = gameWorldMap;

            this.startPanel = new SceneStartPanel(String.Format("GET READY TO MAP {0}", sceneIndex));
            this.completation = new SceneCompletationPanel("GREAT!\nTRY THE NEXT MAP");
            this.closingVignette = new ClosingVignette();
        }

        public override void Initialize()
        {
            base.Initialize();
            
            this.closingVignette.FadeSpeed = 1.0f;
            this.closingVignette.Color = Color.Black;
            this.closingVignette.OnFinished += (sender, args) =>
            {
                this.SetWorldLayersVisible(false);
                this.SetLayerEnabledAndVisible(LAYER_CLOSING_VIGNETTE, false);
                this.SetLayerEnabledAndVisible(LAYER_OPENING_CONCLUSION, true);
                
                this.completation.Run();
            };

            this.GameController = NoActionGameController.Instance;
            this.startPanelClose = Wait.Seconds(2).Then(BeginSceneGamePlay);
                        
            this.DisableAndHideAllLayers();
            this.SetLayerEnabledAndVisible(LAYER_OPENING_CONCLUSION, true);

            this.startPanel.Enable();
            this.completation.Disable();
            this.startPanelClose.Start();
        }

        public override void LoadContent()
        {
            this.AddChild(LAYER_OPENING_CONCLUSION, this.startPanel);
            this.AddChild(LAYER_OPENING_CONCLUSION, this.completation);
            this.AddChild(LAYER_CLOSING_VIGNETTE, this.closingVignette);

            GameManager.Sound.LoadSFX(Constants.SFXAssets.ACHIEVEMENT);
            GameManager.Sound.LoadSong(Constants.SongAssets.BG_SONG);

            this.World = new GameWorld(this.gameWorldMap);
            this.Player = new Player(this.World, 100.0f, 3.0f);

            this.AddChild(LAYER_WORLD_0, this.World);
            this.AddChild(LAYER_WORLD_1, this.Player);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if(this.startPanelClose != null)
                this.startPanelClose.Update(gameTime);
        }

        public override void Complete()
        {
            base.Complete();

            GameManager.Input.MainGameController = NoActionGameController.Instance;

            this.SetWorldLayersEnabled(false);
            this.SetLayerEnabledAndVisible(LAYER_CLOSING_VIGNETTE, true);
            //this.closing.Position = this.Player.Center;
                        
            GameManager.Sound.StopCurrentSounds();
            GameManager.Sound.PlaySFX(Constants.SFXAssets.ACHIEVEMENT);
        }

        private void BeginSceneGamePlay(GameTime t)
        {
            this.startPanel.Disable();
            this.startPanelClose = null;
                        
            this.GameController = GameManager.Input.HandControllerToPlayer(this.Player);
            this.Player.Position = this.World.Start.Bounds.BoundingRectangle.Center;

            GameManager.MainCamera.MovementBoundaries = this.World.Bounds;
            GameManager.MainCamera.LookAt(this.Player.Center);

            GameManager.Sound.PlaySong(Constants.SongAssets.BG_SONG);


            this.DisableAndHideAllLayers();
            this.SetWorldLayersVisible(true);
            this.SetWorldLayersEnabled(true);

            this.VisualDebugMode = false;            
        }
    }
}
