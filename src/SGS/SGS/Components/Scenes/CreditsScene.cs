using Microsoft.Xna.Framework;
using SGS.Components.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace SGS.Components.Scenes
{
    public class CreditsScene : GameScene
    {
        private ScrollingLabel credits;
        private ScrollingLabel endingLabel;
        private SpriteFont creditsFont;
        private SpriteFont creditsEndingFont;
        private Texture2D fadingEffectTexture;
        private float textBoxWidth;

        public CreditsScene()
            : base()
        {
            this.BackgroundColor = Color.Black;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            creditsFont = GameManager.Content.Load<SpriteFont>(Label.Fonts.CREDITS);
            creditsEndingFont = GameManager.Content.Load<SpriteFont>(Label.Fonts.CREDITS_ENDING);

            credits = new ScrollingLabel(creditsFont);
            endingLabel = new ScrollingLabel(creditsEndingFont);

            credits.LoadContent();
            endingLabel.LoadContent();

            this.AddChild(credits);
            this.AddChild(endingLabel);

            this.fadingEffectTexture = GameManager.Content.Load<Texture2D>("sprites/fadingEffect");
            this.credits.Text = GameManager.Content.Load<String>("text/end_credits").Trim();

            GameManager.Sound.LoadSong(Constants.SongAssets.CREDITS_SONG);
        }

        public override void Initialize()
        {
            base.Initialize();

            GameManager.Sound.PlaySong(Constants.SongAssets.CREDITS_SONG);
            MediaPlayer.IsRepeating = false;
                        
            textBoxWidth = creditsFont.MeasureString(credits.Text).X;
            credits.Position = new Vector2((GameManager.GraphicsDevice.Viewport.Width / 2) - (textBoxWidth / 2), GameManager.GraphicsDevice.Viewport.Height);
            credits.Color = Color.LightYellow;
            credits.AlignmentFlags = Label.Alignment.Center;
            credits.ScrollDirection = ScrollingLabel.Direction.UP;
            credits.ScrollSpeed = 50f;
            credits.Initialize();

            endingLabel.Text = "The End";
            textBoxWidth = creditsEndingFont.MeasureString(endingLabel.Text).X;
            endingLabel.Position = new Vector2((GameManager.GraphicsDevice.Viewport.Width / 2) - (textBoxWidth / 2), credits.DestinationRectangle.Bottom + 150);
            endingLabel.Color = Color.LightYellow;
            endingLabel.AlignmentFlags = Label.Alignment.Center;
            endingLabel.ScrollDirection = ScrollingLabel.Direction.UP;
            endingLabel.ScrollSpeed = 50f;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (endingLabel.Position.Y + (endingLabel.DestinationRectangle.Height / 2) <= GameManager.GraphicsDevice.Viewport.Height / 2)
                endingLabel.StopScrolling();

            if (MediaPlayer.State == MediaState.Stopped)
                GameManager.PlayNextScene();
        }

        public override void Draw(SpriteBatch canvas)
        {
            base.Draw(canvas);

            canvas.Begin();
            canvas.Draw(fadingEffectTexture, position: new Vector2(GameManager.GraphicsDevice.Viewport.Width / 2, fadingEffectTexture.Height / 2),
                scale: new Vector2(10, 1), origin: fadingEffectTexture.Bounds.Center.ToVector2());
            canvas.Draw(fadingEffectTexture,
                position: new Vector2(GameManager.GraphicsDevice.Viewport.Width / 2, GameManager.GraphicsDevice.Viewport.Height - (fadingEffectTexture.Height / 2)),
                scale: new Vector2(10, 1), origin: fadingEffectTexture.Bounds.Center.ToVector2(), rotation: (float)Math.PI);
            canvas.End();
        }
    }
}
