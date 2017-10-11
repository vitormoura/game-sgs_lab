using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Shapes;
using Microsoft.Xna.Framework;
using SGS.Components.Players;
using SGS.Components.Scenes;
using Microsoft.Xna.Framework.Graphics;

namespace SGS.Components.UI
{
    public class HealthBar : DrawableGameObject 
    {
        private Player player;
        private const float height = 5.0f;
        private Vector2 position;
        private float width;

        public HealthBar(Player p)
        {
            this.player = p;
        }
                
        public override void Update(GameTime t)
        {
            var posX = this.player.Center.X - (this.player.BoundingBox.Width / 2);
            var posY = this.player.Position.Y - (this.player.Bounds.Height);

            this.width = this.player.BoundingBox.Width;
            this.position = new Vector2(posX, posY);
        }

        public override void Draw(SpriteBatch canvas)
        {
            canvas.DrawRectangle(new RectangleF(this.position.X - 1, this.position.Y - 1, this.width + 2, height + 2), Color.Black);
            canvas.FillRectangle(new RectangleF(this.position.X, this.position.Y, this.width, height), Color.Gray);
            canvas.FillRectangle(new RectangleF(this.position.X, this.position.Y, (this.player.State.CalculateHealthPerc() / 100.0f) * this.width, height), this.player.State.HurtEffect != null ? Color.Red : Color.Green);
        }
    }
}
