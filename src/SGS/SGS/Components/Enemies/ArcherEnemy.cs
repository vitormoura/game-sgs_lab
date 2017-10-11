using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended.Collisions;
using Microsoft.Xna.Framework;
using SGS.Components.Tasks;
using SGS.Components;
using SGS.Components.Scenes;
using SGS.Components.World;

namespace SGS.Components.Enemies
{
    public class ArcherEnemy : Enemy
    {
        private Repeat action;
        private Arrow arrow;
                
        public ArcherEnemy(GameWorld world, Vector2 initialPos, Vector2 direction)
            : base("sprites/orc-bow.ss", world, initialPos)
        {
            this.Direction = direction;

            Random rnd = new Random(DateTime.Now.Millisecond);
            var frequency = rnd.Next(5) + 1;
            
            this.arrow = new Arrow(this.Direction);
            this.action = Repeat.Task(Shoot).Each(frequency * 1000);

            this.AddChild(this.arrow);
        }

        public override void Initialize()
        {
            base.Initialize();

            this.arrow.Disable();
            this.action.Start();

            this.World.RegisterCollisionActor(this.arrow);
            this.World.RegisterCollisionActor(this);
        }
                
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
                        
            this.action.Update(gameTime);
        }

        private void Shoot(GameTime t)
        {
            base.Shoot(onEnd: () => {

               this.arrow.Reset(this.Center);
               this.arrow.Enable();

                this.Stand();
           });
        }

        public override void OnCollision(CollisionInfo collisionInfo)
        {
        }
    }
}
