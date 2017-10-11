using Microsoft.Xna.Framework;
using MonoGame.Extended.InputListeners;
using SGS.Components.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using D = System.Diagnostics.Debug;

namespace SGS.Components.Input
{
    /// <summary>
    /// Componente que gerencia opções e quais mecanismos de controle podem
    /// controlar elementos de jogo
    /// </summary>
    public class GameInputManager : GameObject
    {
        public const int QTD_PLAYERS = 1;
        private PlayerController[] players;
        private IGameInputController mainController;
                
        public IGameInputController MainGameController
        {
            get { return this.mainController; }
            set
            {
                D.Assert(value != null);
                this.mainController = value;
            }
        }
                
        public GameInputManager(Game game, IGameInputController initialGameController)
        {
            this.MainGameController = initialGameController;
            this.players = new PlayerController[QTD_PLAYERS];

            //Por padrão, todos os jogadores são controlados pelo menos tipo de controller
            for (int i = 0; i < this.players.Length; i++)
            {
                this.players[i] = new KBMousePlayerController();
            }
        }

        /// <summary>
        /// Recupera controlador de personagens padrão
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public PlayerController GetPlayerControllerFor(Player p)
        {
            D.Assert(p.Index >= 0 && p.Index <= this.players.Length);
            D.Assert(p != null);
                        
            var ctrl = this.players[p.Index];
                ctrl.Register(p);

            return ctrl;
        }

        /// <summary>
        /// Define o controlador principal de jogo para o mesmo utilizado para controlar o player
        /// </summary>
        /// <param name="p"></param>
        public IGameInputController HandControllerToPlayer(Player p)
        {
            this.MainGameController = this.GetPlayerControllerFor(p);
            return this.MainGameController;
        }
                
        public override void Update(GameTime t)
        {
            this.MainGameController.Update(t);            
        }
    }
}
