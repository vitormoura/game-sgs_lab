using Microsoft.Xna.Framework;
using SGS.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended.Shapes;
using MonoGame.Extended.Collisions;
using MonoGame.Extended;
using SGS.Components.Scenes;
using Microsoft.Xna.Framework.Graphics;
using SGS.Components.Players;
using SGS.Components.Sprites;

namespace SGS.Components.Enemies
{
    public class Arrow : DrawableGameObject, IActorTarget
    {
        public static readonly object TAG = "Arrow";
        private Sprite sprite;
                
        private RectangleF bounds;
                
        public Vector2 Direction { get; private set; }

        public Vector2 Velocity { get; set; }
                                
        public RectangleF BoundingBox
        {
            get
            {
                return new RectangleF(this.bounds.X, this.bounds.Y , this.bounds.Width, this.bounds.Height);
            }
        }

        public Arrow(Vector2 dir)
        {
            this.Direction = dir;
            this.Direction.Normalize();
            this.Velocity = this.Direction * (Constants.M * 3);
            this.Tag = Arrow.TAG;
        }

        public void Reset(Vector2 pos)
        {
            this.Position = pos;
        }

        public override void Update(GameTime t)
        {
            this.Position = new Vector2(this.Position.X, this.Position.Y + (Constants.M / 4.0f * (float)t.ElapsedGameTime.TotalSeconds));
            this.sprite.Position = new Vector2(this.Position.X + sprite.Bounds.Width * this.Direction.X, this.Position.Y);
            this.sprite.Effects = this.Direction.X > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            this.bounds = new RectangleF(this.sprite.Position.X, this.Position.Y + sprite.Bounds.Height / 2, sprite.Bounds.Width, 2);
            //this.Position = this.sprite.Position;
        }

        public override void Draw(SpriteBatch canvas)
        {
            sprite.Draw(canvas);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            this.sprite = new Sprite("sprites/arrow");
            this.sprite.LoadContent();
        }

        public void OnCollision(CollisionInfo collisionInfo)
        {
            if (collisionInfo.Other.Tag == Player.TAG)
            {
                this.Disable();
            }
        }
    }
}
