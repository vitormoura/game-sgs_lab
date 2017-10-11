using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Shapes;
using SGS.Components;
using SGS.Components.Scenes;
using SGS.Components.Sprites;
using SGS.Components.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.Components.Enemies
{
    public class Bomb : DrawableGameObject, IActorTarget
    {
        public enum States
        {
            Ready,
            Flying,
            Exploding
        }

        private Vector2 owner;

        private Vector2 initialPosition;
        private Vector2 direction;
        private Vector2 target;
        private float distance;
        private Vector2 currentThrowForce;

        private SpriteAnimation flyingAnim;
        private SpriteAnimation explosionAnim;
        private SpriteAnimation currentAnimation;

        private float flyingElapsedTime;

        public States State { get; private set; }

        public Vector2 Target
        {
            get { return this.target; }
        }

        public Vector2 Velocity
        {
            get;
            set;
        }

        public RectangleF BoundingBox
        {
            get
            {
                if (this.State == States.Exploding)
                {
                    var w = (this.explosionAnim.CurrentFrame.Bounds.Width / 2);
                    var h = (this.explosionAnim.CurrentFrame.Bounds.Height / 2);

                    return new RectangleF(this.Position.X - w  /2  , this.Position.Y - h  /2, this.explosionAnim.CurrentFrame.Bounds.Width / 2, this.explosionAnim.CurrentFrame.Bounds.Height / 2);
                }
                else
                    return RectangleF.Empty;
            }
        }

        public Bomb(Vector2 owner, Vector2 direction, float distance)
        {
            this.owner = owner;
            this.distance = distance;
            this.direction = direction;
            this.State = States.Ready;
            this.Tag = Enemy.TAG;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            var sprites = new SpriteSheet("sprites/bomb.ss", new SpriteSheetParams(0, 0, 384, 128, 6, 2));
            sprites.LoadContent();


            this.flyingAnim = new SpriteAnimation(sprites.GetSpriteSequenceOfLine(0, 6));
            this.explosionAnim = new SpriteAnimation(sprites.GetSpriteSequenceOfLine(1, 6), loop: false);

            this.AddChild(this.flyingAnim);
            this.AddChild(this.explosionAnim);
        }

        public void Throw()
        {
            this.target = new Vector2(this.initialPosition.X + (this.distance * this.direction.X), this.initialPosition.Y);
            this.initialPosition = this.owner;
            this.Position = this.initialPosition;

            var angle = MathHelper.ToRadians(45);
            var f = (float)Math.Sqrt(this.distance * Constants.DEFAULT_G / Math.Sin(2 * angle));

            this.currentThrowForce = new Vector2(f, 0);
            this.currentThrowForce = this.currentThrowForce.Rotate(angle);
            this.currentThrowForce.X *= this.direction.X;
            this.flyingElapsedTime = 0.0f;

            this.State = States.Flying;

            this.currentAnimation = this.flyingAnim;
            this.currentAnimation.Play();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (State == States.Flying)
            {
                //Já atingimos o objetivo?
                if (flyingElapsedTime > 0 && (this.Position.Y > this.owner.Y))
                {
                    this.State = States.Exploding;

                    this.currentAnimation = this.explosionAnim;
                    this.currentAnimation.Play(Exploded);
                }
                else
                {
                    var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

                    currentThrowForce.Y -= Constants.DEFAULT_G * dt;

                    this.Position = new Vector2(
                        this.Position.X + currentThrowForce.X * dt,
                        this.Position.Y + currentThrowForce.Y * -1 * dt
                    );

                    flyingElapsedTime += dt;
                }
            }


            if (State != States.Ready)
            {
                this.currentAnimation.CurrentFrame.Center = this.Position;
            }
        }

        public void Exploded()
        {
            this.flyingAnim.Disable();
            this.State = States.Ready;
        }

        public override void Draw(SpriteBatch canvas)
        {
            if (State != States.Ready)
            {
                this.currentAnimation.CurrentFrame.Draw(canvas);
            }
        }

        public void OnCollision(CollisionInfo collisionInfo)
        {

        }
    }
}
