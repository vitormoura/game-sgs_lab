using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Shapes;
using SGS.Components.Scenes;
using SGS.Components.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.Components.UI
{
    public class PositionIndicator : DrawableGameObject
    {
        public Wait hideIndicator;
                
        public PositionIndicator()
        {
            this.Visible = false;
        }

        public override void Draw(SpriteBatch canvas)
        {
            if (this.Visible)
            {
                canvas.FillRectangle(this.Position, new Vector2(10, 10), new Color(250, 100, 100));
            }
        }

        public override void Update(GameTime t)
        {
            if (hideIndicator != null)
                hideIndicator.Update(t);
        }

        public void Show(Vector2 pos, Int32 duration)
        {
            if (this.hideIndicator != null && !this.hideIndicator.Finished)
            {
                this.hideIndicator.Stop();
                this.hideIndicator = null;
            }

            this.Position = pos;
            this.Visible = true;

            this.hideIndicator = Wait.Milliseconds(duration * 1000).Then(t =>
            {
                this.Visible = false;
            });
            this.hideIndicator.Start();
        }
    }
}
