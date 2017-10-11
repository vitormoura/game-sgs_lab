using Microsoft.Xna.Framework;
using MonoGame.Extended.Collisions;
using SGS.Components;
using Microsoft.Xna.Framework.Content;
using SGS.Components.Debug;
using SGS.Components.UI;
using Microsoft.Xna.Framework.Audio;
using System;
using SGS.Components.Sprites;
using SGS.Components.Enemies;
using SGS.Components.Scenes;
using SGS.Components.Input;
using SGS.Components.World;
using System.Collections.Generic;
using System.Linq;
using SGS.Components.Tasks;

namespace SGS.Components.Players
{
    public class Player : Character
    {
        public event EventHandler PlayerDied;
        public event EventHandler PlayerRespawn;

        public static readonly object TAG = "Player";

        private const int DEFAULT_RESPAWN_INVENCIBILITY = 1;
                
        private GameWorld world;
        private float defaultSpeed;
        private float speedModifier;
        private Vector2 destination;
        private PlayerStates states;
        private IMessageDebugService debug;
        private IVisualDebugService visualDebug;
        
        private PositionIndicator destinationIndicator;
        private PlayerMind mind;
        private float turnSpeed;
        private Wait invencibilityTimeout;
        private bool falling;
        private bool sliding;
        private bool reachedDestination;

        public Int32 Index { get; set; }

        public float DefaultSpeed
        {
            get { return this.defaultSpeed; }
        }

        public GameWorldArea CurrentArea
        {
            get;
            private set;
        }

        public PlayerStates State
        {
            get { return this.states; }
        }


        public Player(GameWorld world, float defaultMaxHealth, float defaultSpeed)
            : base("sprites/mainplayer.ss")
        {
            this.world = world;
            this.Direction = new Vector2(0, 1);
            this.Velocity = Vector2.Zero;
            this.Enabled = true;
            this.Visible = true;
            this.defaultSpeed = defaultSpeed;
            this.states = new PlayerStates(this, defaultMaxHealth);
            this.speedModifier = 1f;
            this.sliding = false;
            this.reachedDestination = false;
            this.turnSpeed = 0.05f;
        }

        public override void Initialize()
        {
            base.Initialize();

            this.invencibilityTimeout = Wait.Seconds(DEFAULT_RESPAWN_INVENCIBILITY).Then((t) => this.State.Invencible = false);
            this.world.RegisterCollisionActor(this);
            this.Respawn();
        }

        public override void LoadContent()
        {
            this.mind = new PlayerMind(this);
                        
            this.debug = GameManager.Services.GetService<IMessageDebugService>();
            this.visualDebug = GameManager.Services.GetService<IVisualDebugService>();
            GameManager.Sound.LoadSFX(Constants.SFXAssets.PLAYER_DEATH);
            GameManager.Sound.LoadSFX(Constants.SFXAssets.FALLING);
                        
            this.destinationIndicator = new PositionIndicator();
                        
            this.AddChild(this.destinationIndicator);
            this.AddChild(this.mind);

            base.LoadContent();
        }
        
        public void MoveTo(Vector2 p)
        {
            if (State.Alive && !this.falling)
            {

                if (Vector2.Distance(p, this.Position) > 1.0f)
                {
                    this.destination = p;
                    this.Direction = p - this.Position;
                    this.states.Moving = true;

                    this.Walk();
                }


                //TODO: Rever regras para exibição do indicador
                this.destinationIndicator.Show(p, 1);
            }
        }

        public void Stop()
        {
            if (this.State.Moving)
            {
                State.Moving = false;
                this.Velocity = Vector2.Zero;

                this.Stand();
            }
        }

        public void Respawn()
        {
            this.State.Reset();
            this.Velocity = Vector2.Zero;
            this.State.Invencible = true;
            this.invencibilityTimeout.Restart();
            this.Position = this.world.Start.Bounds.BoundingRectangle.Center;
            this.falling = false;
            //this.CurrentAnimation = this.Animations.Spawn();
            //this.CurrentAnimation.Play();

            if (this.PlayerRespawn != null)
                this.PlayerRespawn(this, null);
        }

        public void Enter(GameWorldArea area)
        {
            if (this.CurrentArea == null)
            {
                this.CurrentArea = area;

                if (this.CurrentArea != null)
                {
                    this.speedModifier = this.speedModifier * this.CurrentArea.SpeedModifier;

                    if (this.CurrentArea.DegenerationRatio > 0.0f)
                    {
                        this.states.HurtEffect = new PlayerHurtEffect(this, this.CurrentArea);
                        this.states.HurtEffect.Start();
                    }

                    this.sliding = this.CurrentArea.Slippery;
                }
            }
        }

        public void Exit(GameWorldArea area)
        {
            if (this.CurrentArea != null)
            {
                this.CurrentArea = null;
                this.speedModifier = this.speedModifier / area.SpeedModifier;

                if (this.states.HurtEffect != null)
                {
                    this.states.HurtEffect.Stop();
                    this.states.HurtEffect = null;
                }

                if (this.reachedDestination || area.Slippery)
                    this.Stop();

                this.sliding = false;
                this.reachedDestination = false;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (State.Alive && !this.falling)
            {
                if (states.Moving)
                {
                    if (!this.sliding || (this.Velocity.Length() == 0))
                        this.Velocity = this.Direction * (this.defaultSpeed * Constants.M) * this.speedModifier;
                    else
                    {
                        this.Velocity = this.GetSlidingVelocity();
                    }

                    if (Vector2.Distance(this.destination, this.Position) <= (Constants.M / 4.0f))
                    {
                        if (!this.sliding)
                            this.Stop();
                        else
                            this.reachedDestination = true;
                    }

                    if (this.sliding)
                    {
                        this.Stand();
                    }
                    else
                    {
                        this.Walk();
                    }
                }
                else
                    this.AnimationPlayer.Play(this.Animations.Stand(this.Direction));

                foreach (var a in this.world.SpecialAreas)
                {
                    var intersects = a.Bounds.Contains(this.Position);

                    if (intersects && (this.CurrentArea == null || this.CurrentArea.ID != a.ID))
                    {
                        this.Enter(a);
                        break;
                    }
                    else if (this.CurrentArea != null && this.CurrentArea.ID == a.ID && !intersects)
                    {
                        this.Exit(a);
                        break;
                    }
                }

                //Chegamos ao ponto de destino?
                if (this.world.Goal.Bounds.Contains(this.Position))
                {
                    GameManager.CurrentSceneCompleted();
                }

                if (this.states.HurtEffect != null)
                {
                    this.states.HurtEffect.Update(gameTime);
                }

                if (!this.State.Alive)
                {
                    this.Die();
                }

                this.invencibilityTimeout.Update(gameTime);
                
                if (State.Invencible)
                {
                    this.Sprite.Color = Color.Red == this.Sprite.Color ? Color.White : Color.Red;
                }
                else
                    this.Sprite.Color = Color.White;
            }

#if DEBUG
            debug.AddMessage("P [{0:0000},{1:0000}]", this.Position.X, this.Position.Y);
            debug.AddMessage("V [{0:0000},{1:0000}]", this.Velocity.X, this.Velocity.Y);
            debug.AddMessage("AREA: {0}", this.CurrentArea != null ? this.CurrentArea.Name : "-");

            visualDebug.AddLine(this.Position, this.Position + this.Velocity, Color.Cyan);
            visualDebug.AddLine(this.Position, this.Position + this.Direction * 10f, Color.GreenYellow);
#endif
        }


        public void FallIntoVoid()
        {
            if (!this.falling)
            {
                GameManager.Sound.PlaySFX(Constants.SFXAssets.FALLING);
                this.falling = true;
                this.State.Moving = false;

                base.FallIntoVoid(() =>
                {
                    this.Respawn();
                });
            }
        }

        public void Die()
        {
            this.Velocity = Vector2.Zero;
            this.State.Moving = false;

            base.Die(() =>
            {

                if (this.PlayerDied != null)
                    this.PlayerDied(this, null);

                this.Respawn();

                GameManager.PlayerDied();
            });

            GameManager.Sound.PlaySFX(Constants.SFXAssets.PLAYER_DEATH);
        }

        public override void OnCollision(CollisionInfo collisionInfo)
        {
            if (this.State.Alive)
            {
                if (collisionInfo.PenetrationVector != Vector2.Zero)
                {
                    if (collisionInfo.Other is CollisionGridCell)
                    {
                        var turnAngle = GetAngleBetween(this.Direction, this.destination);
                        var other = (CollisionGridCell)collisionInfo.Other;
                        var penetration = collisionInfo.PenetrationVector;
                                                                                                
                        this.Position -= penetration;
                        
                        //TODO: Como identificar se devemos prosseguir?
                        //this.Stop();

                    }
                    else if (collisionInfo.Other.Tag == Enemy.TAG || collisionInfo.Other.Tag == Arrow.TAG)
                    {
                        this.State.Damage(this.State.MaxHealth);
                                                
                        if (!this.State.Alive)
                        {
                            this.Die();
                        }
                    }
                }
            }
        }

        private Vector2 GetSlidingVelocity()
        {
            float angle = GetTurnAngle();

            angle = MathHelper.Lerp(0.0f, (float)angle, this.turnSpeed);

            var matrix = Matrix.CreateRotationZ((float)angle);

            return Vector2.Transform(this.Velocity, matrix);
        }

        private float GetTurnAngle()
        {
            int mod = 1;
            Vector3 v = Vector3.Cross(new Vector3(this.Velocity, 0.0f), new Vector3(this.Direction, 0.0f));
            mod = v.Z > 0 ? 1 : -1;

            var dot = Vector2.Dot(this.Direction, this.Velocity);
            var cos = dot / (this.Velocity.Length() * this.Direction.Length());
            cos = MathHelper.Clamp(cos, -1, 1);

            float angle = (float)Math.Acos(cos) * mod;
            return angle;
        }

        private float GetAngleBetween(Vector2 v1, Vector2 v2)
        {
            Vector3 v   = Vector3.Cross(new Vector3(v1, 0.0f), new Vector3(v2, 0.0f));
            var mod     = v.Z > 0 ? 1 : -1;
            var dotP = Vector2.Dot(v1, v2);

            /*
            var cos = dotP / (v1.Length() * v2.Length());
            return MathHelper.ToDegrees((float)Math.Acos(cos * mod));
            */
            
            ///* 360
            var determinant = v1.X * v2.Y - v1.Y * v2.X;
            return MathHelper.ToDegrees((float)Math.Atan2(determinant, dotP));
            //*/
        }
    }
}
