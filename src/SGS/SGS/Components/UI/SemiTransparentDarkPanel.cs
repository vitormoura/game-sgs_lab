using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.Components.UI
{
    public class SemiTransparentDarkPanel : DrawableGameObject
    {
        private Texture2D texture;

        public SemiTransparentDarkPanel()
        {
        }

        public override void LoadContent()
        {
            this.texture = GameManager.Content.Load<Texture2D>("sprites/blackbox");
        }

        public override void Draw(SpriteBatch canvas)
        {
            canvas.Draw(this.texture, GameManager.GraphicsDevice.Viewport.Bounds, texture.Bounds, Color.Black * 0.5f );
        }
    }
}
