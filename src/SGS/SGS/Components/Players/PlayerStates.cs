using Microsoft.Xna.Framework;
using SGS.Components;
using SGS.Components.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.Components.Players
{
    public class PlayerStates
    {
        public Player Player { get; private set; }

        public PlayerHurtEffect HurtEffect { get; set; }

        public Int32 Tries { get; private set; }

        public Boolean Invencible { get; set; }
        
        public float CurrentHealth
        {
            get;
            private set;
        }

        public float MaxHealth
        {
            get;
            private set;
        }
                
        public Boolean Moving
        {
            get;
            set;
        }

        public Boolean Alive
        {
            get { return this.CurrentHealth > 0.0f; }
        }

        public PlayerStates(Player p, float maxHealth)
        {
            this.Player = p;
            this.MaxHealth = maxHealth;

            this.Reset();
        }
        
        public float CalculateHealthPerc()
        {
            return (this.CurrentHealth / this.MaxHealth) * 100.0f;
        }

        public Boolean Damage(float value)
        {
            if (!this.Invencible)
            {
                this.CurrentHealth -= value;

                if (this.CurrentHealth < 0.0f)
                {
                    this.CurrentHealth = 0.0f;

                    if (this.Tries > 0)
                    {
                        this.Tries--;
                    }

                    return true;
                }
            }

            return false;

        }

        public void Reset()
        {
            this.CurrentHealth = this.MaxHealth;

            if (this.HurtEffect != null)
                this.HurtEffect.Stop();

            this.HurtEffect = null;
            this.Moving = false;
        }
    }

}
