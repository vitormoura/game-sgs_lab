using Microsoft.Xna.Framework;
using SGS.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Shapes;
using SGS.Components.Tasks;
using Microsoft.Xna.Framework.Audio;
using SGS.Components.Debug;
using SGS.Components.Sprites;
using SGS.Components.Scenes;
using Microsoft.Xna.Framework.Graphics;
using SGS.Components.World;

namespace SGS.Components.Enemies
{
    public class CannonEnemy : DrawableGameObject
    {
        private enum States
        {
            Ready,
            Firing,
            Reloading
        }

        private const int ANIM_STANDBY = 0;
        private const int ANIM_BLAST = 1;

        private Bomb bomb;
        private Vector2 position;
        private Wait fireBombTask;
        private GameWorld world;
        private SpriteAnimation[] animations;
        private SpriteAnimation currentAnimation;
                
        
        private CannonEnemyParams parameters;
        private IVisualDebugService debug;


        public CannonEnemy(GameWorld world, Vector2 pos, CannonEnemyParams param)
        {
            this.position = pos;
            this.parameters = param;
            this.world = world;
        }

        public override void Initialize()
        {
            base.Initialize();

            
            this.currentAnimation = this.animations[ANIM_STANDBY];
            this.currentAnimation.Play();

            this.bomb.Initialize();
            //this.fireBombTask.Start();

            this.world.RegisterCollisionActor(this.bomb);
        }

        public override void LoadContent()
        {
            var sprites = new SpriteSheet("sprites/cannon.ss", new SpriteSheetParams(0, 0, 192, 192, 3, 3));
            sprites.LoadContent();
            sprites.Initialize();

            var direction = this.parameters.Direction;

            var animDirection = direction.X == -1 ? 0 : 1;
            var spritesAnimBlast = sprites.GetSpriteSequenceOfLine(animDirection, 3);

            this.animations = new SpriteAnimation[2];
            this.animations[ANIM_BLAST] = new SpriteAnimation(spritesAnimBlast, speed: 25, loop: false);
            this.animations[ANIM_STANDBY] = new SpriteAnimation(new Sprite[] { spritesAnimBlast[0] });

            var cannonSourcePos = this.position;

            if (direction.X == 1)
            {
                cannonSourcePos.X += this.animations[ANIM_STANDBY].CurrentFrame.Bounds.Width;
            }
                        
            this.bomb = new Bomb(cannonSourcePos, direction, Constants.M * parameters.DistanceOfShot);
            this.bomb.LoadContent();

            GameManager.Sound.LoadSFX(Constants.SFXAssets.BOMB_EXPLOSION);

            this.fireBombTask = Wait.Milliseconds((int)this.parameters.ThrowFrequency.TotalMilliseconds).Then(Fire);
            this.debug = GameManager.Services.GetService<IVisualDebugService>();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.currentAnimation.Update(gameTime);
            this.currentAnimation.CurrentFrame.Position = this.position;
            this.fireBombTask.Update(gameTime);

            if (this.bomb.Enabled)
            {
                this.bomb.Update(gameTime);

                if (this.bomb.State == Bomb.States.Ready)
                {
                    this.Fire(gameTime);
                    //this.fireBombTask.Restart();
                }
                    
                                                                               
#if DEBUG
                debug.AddShape(new RectangleF(this.bomb.Target.X, this.bomb.Target.Y, Constants.M / 2.0f, Constants.M / 2.0f), Color.Orange);
#endif
            }
        }

        public override void Draw(SpriteBatch canvas)
        {
            this.currentAnimation.CurrentFrame.Draw(canvas);

            if (this.bomb.Visible)
                this.bomb.Draw(canvas);
        }
               

        private void Fire(GameTime t)
        {
            GameManager.Sound.PlaySFX(Constants.SFXAssets.BOMB_EXPLOSION);

            this.bomb.Throw();

            this.currentAnimation = this.animations[ANIM_BLAST];
            this.currentAnimation.Play(() =>
            {
                this.currentAnimation = this.animations[ANIM_STANDBY];
            });
        }
    }
}
