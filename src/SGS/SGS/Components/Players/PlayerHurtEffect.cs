using Microsoft.Xna.Framework;
using SGS.Components;
using SGS.Components.Tasks;
using SGS.Components.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.Components.Players
{
    public class PlayerHurtEffect 
    {
        private Player player;
        private GameWorldArea area;
        private Repeat repetition;

        public PlayerHurtEffect(Player p, GameWorldArea area)
        {
            this.player = p;
            this.area = area;
            this.repetition = Repeat.Task(HurtPlayer).Each(1000);
        }

        public void Update(GameTime t)
        {
            this.repetition.Update(t);
        }

        public void Start()
        {
            this.repetition.Start();
        }

        public void Stop()
        {
            this.repetition.Stop();
        }

        private void HurtPlayer(GameTime t)
        {
            this.player.State.Damage(this.area.DegenerationRatio);
        }
    }
}
