using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.Components.Enemies
{
    public class CannonEnemyParams
    {
        public TimeSpan ThrowFrequency { get; set; }

        public Vector2 Direction { get; set; }

        public float DistanceOfShot { get; set; }
    }
}
