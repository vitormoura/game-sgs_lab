using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace SGS.Components.UI
{
    public class PositionIndicator2 : DrawableGameObject
    {
        private Texture2D clickIndicator;
        private Texture2D indicator;

        private bool showClickIndicator;

        public PositionIndicator2()
        {
        }

        public override void Draw(SpriteBatch canvas)
        {
            if (this.showClickIndicator)
            {
                canvas.Draw(this.clickIndicator, this.Position, this.clickIndicator.Bounds, Color.White);
            }
            else
            {
                canvas.Draw(this.indicator, this.Position, this.indicator.Bounds, Color.White);
            }
        }

        public override void LoadContent()
        {
            base.LoadContent();

            this.clickIndicator = GameManager.Content.Load<Texture2D>("sprites/mouse_click");
            this.indicator = GameManager.Content.Load<Texture2D>("sprites/mouse_position");
        }

        public override void Update(GameTime t)
        {
            var state = Mouse.GetState();
            var mousePos = state.Position.ToVector2();

            this.Position = mousePos;
            this.showClickIndicator = state.LeftButton == ButtonState.Pressed;
        }

        public void Show(Vector2 pos, Int32 duration)
        {
        }
    }
}
