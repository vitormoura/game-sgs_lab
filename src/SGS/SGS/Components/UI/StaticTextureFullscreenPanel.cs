using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.Components.UI
{
    public class StaticTextureFullscreenPanel : DrawableGameObject
    {
        private Rectangle textureBounds;
        private Texture2D background;
        private String texture;

        public FancyLabel Message { get; private set; }

        public StaticTextureFullscreenPanel(String texture)
        {
            this.texture = texture;
            this.textureBounds = Rectangle.Empty;
            this.Message = new FancyLabel { Rotation = 0.1f };
        }

        public override void Initialize()
        {
            base.Initialize();

            this.Position = new Vector2((GameManager.GraphicsDevice.Viewport.Width - this.textureBounds.Width) / 2, (GameManager.GraphicsDevice.Viewport.Height - this.textureBounds.Height) / 2);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            this.Message.LoadContent();
            this.background = GameManager.Content.Load<Texture2D>(texture);
            this.textureBounds = this.background.Bounds;
        }

        public override void Update(GameTime t)
        {
            this.Position = new Vector2((GameManager.GraphicsDevice.Viewport.Width - this.textureBounds.Width) / 2, (GameManager.GraphicsDevice.Viewport.Height - this.textureBounds.Height) / 2);
        }

        public override void Draw(SpriteBatch canvas)
        {
            
                canvas.Draw(this.background, this.Position, this.background.Bounds, Color.White);

                if (!String.IsNullOrEmpty(this.Message.Text))
                {
                    this.Message.Position = this.Position;
                    this.Message.Draw(canvas);
                }
            
        }
    }
}
