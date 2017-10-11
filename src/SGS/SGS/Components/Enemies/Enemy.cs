using Microsoft.Xna.Framework;
using SGS.Components;
using SGS.Components.Scenes;
using SGS.Components.Sprites;
using SGS.Components.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.Components.Enemies
{
    public abstract class Enemy : Character
    {
        public static readonly object TAG = "Enemy";

        public GameWorld World { get; private set; }

        public Enemy(string spriteSheet, GameWorld world, Vector2 pos)
            : base(spriteSheet, pos)
        {
            this.World = world;
            this.Tag = Enemy.TAG;
            this.Position = pos;
        }
    }
}
