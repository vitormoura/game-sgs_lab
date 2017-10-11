using Microsoft.Xna.Framework;
using SGS.Components.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.Components.Players
{
    /// <summary>
    /// Componente capaz de invocar interações junto ao jogador
    /// </summary>
    public abstract class PlayerController : GameObject, IGameInputController
    {
        public Player Player { get; private set; }

        public virtual void Register(Player p)
        {
            this.Player = p;
        }
    }
}
