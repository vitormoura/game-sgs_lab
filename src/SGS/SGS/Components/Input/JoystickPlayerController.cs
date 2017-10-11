using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SGS.Components.Players;
using MonoGame.Extended.InputListeners;

namespace SGS.Components.Input
{
    public class JoystickPlayerController : PlayerController, IGameInputController
    { 
        private Player player;
        private GamePadListener listener;

        public JoystickPlayerController(Player p)
            : base()
        {
            this.player = p;
            this.listener = new GamePadListener(new GamePadListenerSettings { });
            this.listener.ThumbStickMoved += ThumbStickMoved;
        }

        private void ThumbStickMoved(object sender, GamePadEventArgs e)
        {
            //TODO: Implementar movimentação do personagem
        }

        public override void Update(GameTime gameTime)
        {
            this.listener.Update(gameTime);
        }
    }
}
