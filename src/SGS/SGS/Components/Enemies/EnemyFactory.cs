using Microsoft.Xna.Framework;
using SGS.Components.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.Components.Enemies
{
    public static class EnemyFactory
    {
        public const String ENEMY_TP_SHOOTER = "Archer";
        public const String ENEMY_TP_WANDERER = "Wanderer";
        public const String ENEMY_TP_CANNON = "Cannon";
        public const String ENEMY_TP_ROTATE = "Rotate";
        

        public static DrawableGameObject Create(GameWorld world, EnemyCreationParams param)
        {
            DrawableGameObject result = null;

            switch (param.Type)
            {
                case ENEMY_TP_SHOOTER:
                    result = new ArcherEnemy(world, param.Position, Vector2.UnitX * param.Direction);
                    break;

                case ENEMY_TP_CANNON:

                    result = new CannonEnemy(
                        world,
                        param.Position,
                        new CannonEnemyParams
                        {
                            Direction = Vector2.UnitX * param.Direction,
                            DistanceOfShot = param.Force,
                            ThrowFrequency = TimeSpan.FromSeconds(param.Frequency)
                        });
                    break;

                case ENEMY_TP_ROTATE:
                    result = new RotatingPost(world, param.Position, param.Speed.X);
                    break;

                case ENEMY_TP_WANDERER:
                default:
                    result = new WandererEnemy(world, param.Position, Vector2.UnitX * param.Direction);
                    break;
            }

            return result;
        }
    }
}
