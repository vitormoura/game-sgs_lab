using MonoGame.Extended.Collisions;
using MonoGame.Extended.Shapes;
using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace SGS.Components.Sprites
{
    public abstract class Character : DrawableGameObject, IActorTarget
    {
        private Vector2 initialPosition;
        private CharacterAnimations animations;
        
        private Vector2 direction;
        private Vector2 velocity;
        private RectangleF boundingBox;
        private String spriteSheetName;

        /// <summary>
        /// Catálogo de animações disponíveis ao personagem
        /// </summary>
        protected CharacterAnimations Animations
        {
            get { return this.animations; }
        }

        /// <summary>
        /// Responsável por controlar a animação atualmente em curso
        /// </summary>
        protected SpriteAnimationPlayer AnimationPlayer { get; private set; }
        
        /// <summary>
        /// Sprite sendo exibido pela animação corrente
        /// </summary>
        public Sprite Sprite
        {
            get
            {
                return this.AnimationPlayer != null && this.AnimationPlayer.CurrentScene != null ?
                    this.AnimationPlayer.CurrentScene.CurrentFrame :
                    null;
            }
        }
        
        /// <summary>
        /// Direção a qual esse personagem aponta
        /// </summary>
        public Vector2 Direction
        {
            get { return this.direction; }
            protected set { this.direction = value; this.direction.Normalize(); }
        }

        /// <summary>
        /// Representa o ponto central do personagem
        /// </summary>
        public override Vector2 Position
        {
            get { return base.Position; }
            set
            {
                base.Position = value;

                /*
                //Por padrão, sprites de Character têm um espaço vazio entre ele e sua borda, vamos eliminá-lo para efeitos de colisão
                var spritePadding = this.CurrentAnimation.CurrentFrame.Bounds.Width / 4;

                this.boundingBox = new RectangleF(
                    value.X + spritePadding,
                    value.Y + spritePadding,
                    this.CurrentAnimation.CurrentFrame.Bounds.Width / 2,
                    this.CurrentAnimation.CurrentFrame.Bounds.Height - spritePadding);
                //*/

                if (this.Sprite != null)
                {
                    ///*
                    //Por padrão, sprites de Character têm um espaço vazio entre ele e sua borda, vamos eliminá-lo para efeitos de colisão
                    var spritePadding = this.Sprite.Bounds.Width / 4;
                    var width = this.Sprite.Bounds.Width;
                    var height = this.Sprite.Bounds.Height;

                    this.Bounds = new Rectangle(
                        ((int)(base.Position.X - width / 2)),
                        ((int)(base.Position.Y - height / 1.1f)),
                        width,
                        height
                    );

                    this.boundingBox = new RectangleF(
                        this.Bounds.X + spritePadding,
                        this.Bounds.Y + spritePadding + (height / 2),
                        width / 2,
                        height - (spritePadding + (height / 2)));
                    //*/
                }
            }
        }
                
        public Vector2 Velocity
        {
            get { return this.velocity; }
            set { this.velocity = value; }
        }
               
        public RectangleF BoundingBox
        {
            get { return this.boundingBox; }
        }
        

        public Character(String characterSpriteSheet)
            : this(characterSpriteSheet, Vector2.Zero)
        {
        }

        public Character(String characterSpriteSheet, Vector2 initialPos)
        {
            this.direction = Vector2.UnitY;
            this.initialPosition = initialPos;
            this.velocity = Vector2.Zero;
            this.spriteSheetName = characterSpriteSheet;
            this.Bounds = Rectangle.Empty;
        }

        public override void Initialize()
        {
            base.Initialize();
                       
            this.AnimationPlayer.Play(this.animations.Stand(this.direction));

            this.Position = this.initialPosition;
        }

        public override void LoadContent()
        {
            var ssheet = new CharacterSpriteSheet(this.spriteSheetName);
            ssheet.LoadContent();
                        
            this.animations = new CharacterAnimations(ssheet);
            this.AnimationPlayer = new SpriteAnimationPlayer();
            
            this.AddChild(this.AnimationPlayer);
            this.AddChild(this.animations);

            base.LoadContent();
        }

        public override void Update(GameTime t)
        {
            //this.Sprite.Scale = Vector2.One * 2.0f;

            base.Update(t);
        }

        public abstract void OnCollision(CollisionInfo collisionInfo);

        protected virtual void Walk()
        {
            this.AnimationPlayer.Play(this.animations.Walk(this.Direction));
        }

        protected virtual void Stand()
        {
            this.AnimationPlayer.Play(this.animations.Stand(this.Direction));
        }

        protected void Die(Action onEnd = null)
        {
            this.AnimationPlayer.Play(this.animations.Die(), onEnd);
        }

        protected void Shoot(Action onEnd = null)
        {
            this.AnimationPlayer.Play(this.animations.Shoot(this.Direction), onEnd);
        }

        protected void FallIntoVoid(Action onEnd = null)
        {
            this.AnimationPlayer.Play(this.animations.FallIntoVoid(), onEnd);
        }
    }
}
