using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SGS.Components.Input
{
    /// <summary>
    /// Implementação que ignora qualquer input
    /// </summary>
    public class NoActionGameController : IGameInputController
    {
        /// <summary>
        /// Instância única que pode ser utilizada por todos os componentes
        /// </summary>
        public static readonly NoActionGameController Instance = new NoActionGameController();

        private NoActionGameController()
        {
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
