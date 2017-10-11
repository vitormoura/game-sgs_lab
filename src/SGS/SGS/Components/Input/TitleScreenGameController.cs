using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGame.Extended.InputListeners;
using SGS.Components.Scenes;
using Microsoft.Xna.Framework.Input;

namespace SGS.Components.Input
{
    public class TitleScreenGameController : IGameInputController
    {
        private KeyboardListener keyboardListener;
        private TitleScreenScene scene;

        public TitleScreenGameController(TitleScreenScene scene )
        {
            this.scene = scene;
            keyboardListener = new KeyboardListener();
            keyboardListener.KeyPressed += OnKeyPressed;
        }

        private void OnKeyPressed(object sender, KeyboardEventArgs e)
        {
            if (e.Key == Keys.Enter)
                GameManager.CurrentSceneCompleted();
        }

        public void Update(GameTime t)
        {
            this.keyboardListener.Update(t);
        }
    }
}
