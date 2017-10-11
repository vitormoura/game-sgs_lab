using Microsoft.Xna.Framework;
using MonoGame.Extended.Collisions;
using SGS.Components.Tasks;
using SGS.Components.Scenes;
using SGS.Components.World;

namespace SGS.Components.Enemies
{
    public class WandererEnemy : Enemy
    {
        private Wait movement;
        
        public WandererEnemy(GameWorld world, Vector2 pos, Vector2 direction)
            : base("sprites/orc-spear.ss", world, pos)
        {
            this.Direction = direction;
            this.Velocity = direction * 2.0f * Constants.M;
        }

        public override void Initialize()
        {
            base.Initialize();

            this.movement = Wait.Seconds(5).Then(GoBack);
            this.movement.Start();
                        
            this.World.RegisterCollisionActor(this);
            this.Walk();
        }

        public override void OnCollision(CollisionInfo collisionInfo)
        {
            if (collisionInfo.Other is CollisionGridCell)
            {
                this.Position = this.Position - (collisionInfo.PenetrationVector * (Constants.M / 4));
                this.movement.Restart();
                this.GoBack(null);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.movement.Update(gameTime);

            if (this.movement.Finished)
                this.movement.Start();
        }

        private void GoBack(GameTime t)
        {
            this.Direction = this.Direction * -1;
            this.Velocity = this.Velocity * -1;

            this.Walk();
        }
    }
}
