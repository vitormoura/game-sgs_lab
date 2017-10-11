using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGame.Extended.InputListeners;

namespace SGS.Components.Input
{
    public class PauseStateGameController : IGameInputController
    {
        private MouseListener mouseListener;
        private KeyboardListener keyboardListener;

        public PauseStateGameController()
        {
            this.keyboardListener = new KeyboardListener();
            this.mouseListener = new MouseListener();

            this.keyboardListener.KeyReleased += OnKeyReleased;
            this.keyboardListener.KeyPressed += OnKeyPressed;
                        
            this.mouseListener.MouseClicked += OnMouseClick;
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
        }

        private void OnKeyPressed(object sender, KeyboardEventArgs e)
        {
        }

        private void OnKeyReleased(object sender, KeyboardEventArgs e)
        {
            if (e.Key == Microsoft.Xna.Framework.Input.Keys.Enter)
                GameManager.TogglePause();
        }

        public void Update(GameTime t)
        {
            this.keyboardListener.Update(t);
            this.mouseListener.Update(t);
        }
    }
}
