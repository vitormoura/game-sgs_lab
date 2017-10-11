using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SGS.Components.Input;
using SGS.Components.Sprites;
using SGS.Components.Tasks;
using SGS.Components.UI;
using System;

namespace SGS.Components.Scenes
{
    public class TitleScreenScene : GameScene
    {
        private Wait startNextScene;
        private Sprite logo;
        private Sprite warrior;
        private Sprite orc;
        private Sprite currentCharSprite;
        private float fade;
        private BlinkingLabel pressStartLabel;

        private Boolean ending;

        public TitleScreenScene()
            : base()
        {
            this.BackgroundColor = Color.White;
            this.logo = new Sprite("sprites/gamelogo");
            this.warrior = new Sprite("sprites/warrior_silhouette");
            this.orc = new Sprite("sprites/orc_silhouette");
        }

        public override void LoadContent()
        {
            base.LoadContent();

            this.BackgroundColor = Color.Black;

            this.logo.Visible = true;
                        
            this.AddChild(this.warrior);
            this.AddChild(this.orc);
            this.AddChild(this.logo);
            

            this.logo.LoadContent();
            this.warrior.LoadContent();
            this.orc.LoadContent();

            this.pressStartLabel = new BlinkingLabel(GameManager.Content.Load<SpriteFont>(Label.Fonts.DEFAULT)) { Text = "PRESS ENTER TO PLAY" };

            GameManager.Sound.LoadSong(Constants.SongAssets.BG_TITLESCREEN);
                        
            this.AddChild(pressStartLabel);
                        
            GameManager.Sound.LoadSFX(Constants.SFXAssets.START);
        }

        public override void Initialize()
        {
            base.Initialize();

            GameManager.Sound.PlaySong(Constants.SongAssets.BG_TITLESCREEN);

            //Centralizado
            this.logo.Position = new Vector2(GameManager.GraphicsDevice.Viewport.Bounds.Center.X - this.logo.Bounds.Width / 2,
                GameManager.GraphicsDevice.Viewport.Bounds.Center.Y - this.logo.Bounds.Height / 2);

            this.orc.Position =  this.warrior.Position = (this.logo.Position - (this.logo.Position * (Vector2.One * 0.5f)));

            this.pressStartLabel.Position = new Vector2( this.logo.Position.X + this.logo.Bounds.Right - this.logo.Bounds.Width / 2 , this.logo.Position.Y + this.logo.Bounds.Height ); ;
            this.pressStartLabel.Text = "PRESS ENTER TO PLAY";
            this.pressStartLabel.AlignmentFlags = Label.Alignment.Center | Label.Alignment.Bottom;
            this.pressStartLabel.Color = Color.White;

            this.GameController = new TitleScreenGameController(this);
            this.currentCharSprite = this.warrior;
            this.warrior.Visible = true;
            this.orc.Visible = false;

            this.ending = false;
            this.startNextScene = Wait.Milliseconds(2000).Then((t) =>
            {
                GameManager.PlayNextScene();
            });

            GameManager.Sound.PlaySong(Constants.SongAssets.BG_TITLESCREEN);
            GameManager.MainCamera.Reset();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (this.ending)
            {
                this.startNextScene.Update(gameTime);
            }
            else
            {
                ///*
                this.fade += (0.20f * (float)gameTime.ElapsedGameTime.TotalSeconds);

                this.currentCharSprite.Color = Color.White * (1.0f - this.fade);

                if (this.fade >= 1.0f)
                {
                    this.fade = 0.0f;

                    if (this.currentCharSprite == this.warrior)
                    {
                        this.currentCharSprite = this.orc;
                        this.warrior.Visible = false;
                        this.orc.Visible = true;
                    }
                    else
                    {
                        this.currentCharSprite = this.warrior;
                        this.warrior.Visible = true;
                        this.orc.Visible = false;
                    }
                }
                //*/
            }
        }

        public override void Complete()
        {
            base.Complete();

            this.ending = true;
            GameManager.Sound.PlaySFX(Constants.SFXAssets.START);
            GameManager.Sound.StopCurrentSounds();

            this.startNextScene.Start();
        }
    }
}
