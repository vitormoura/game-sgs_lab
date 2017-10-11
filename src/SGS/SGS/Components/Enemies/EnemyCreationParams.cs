using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.Components.Enemies
{
    public class EnemyCreationParams
    {
        public String Type { get; set; }

        public Vector2 Position { get; set; }

        public Int32 Direction { get; set; }

        public Vector2 Speed { get; set; }

        public Double Frequency { get; set; }

        public float Force { get; set; }

        private EnemyCreationParams()
        {
        }

        public static EnemyCreationParams Parse(Dictionary<String, String> properties)
        {
            EnemyCreationParams param = new EnemyCreationParams();

            String type;

            if (properties.TryGetValue("class", out type))
            {
                param.Type = type;
            }
            else
                param.Type = EnemyFactory.ENEMY_TP_WANDERER;

            String direction;

            if (properties.TryGetValue("direction", out direction))
            {
                param.Direction = int.Parse(direction);
            }
            else
                param.Direction = (int)Vector2.UnitX.X;

            String distance;

            if (properties.TryGetValue("distance", out distance))
            {
                param.Force = float.Parse(distance, CultureInfo.InvariantCulture);
            }
            


            String frequency;

            if (properties.TryGetValue("frequency", out frequency))
            {
                param.Frequency = double.Parse(frequency, CultureInfo.InvariantCulture);
            }

            String speed;

            if (properties.TryGetValue("speed", out speed))
            {
                var s = float.Parse(speed, CultureInfo.InvariantCulture);
                param.Speed = new Vector2(s, s);
            }
            else
                param.Speed = Vector2.Zero;


            return param;
        }
    }
}
