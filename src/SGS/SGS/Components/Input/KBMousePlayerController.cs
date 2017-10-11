using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.InputListeners;
using SGS.Components.Players;
using System;

namespace SGS.Components.Input
{
    public class KBMousePlayerController : PlayerController
    {
        private const float CAMERA_DRAG_SPEED = 10.0f;

        private MouseListener mouseListener;
        private KeyboardListener keyboardListener;
                

        public KBMousePlayerController()
        {
            this.keyboardListener = new KeyboardListener();
            this.mouseListener = new MouseListener();

            this.keyboardListener.KeyReleased += OnKeyReleased;
            this.keyboardListener.KeyPressed += OnKeyPressed;

            this.mouseListener.MouseDown += OnMouseDown;
            this.mouseListener.MouseWheelMoved += OnMouseWheelMoved;
            this.mouseListener.MouseDrag += OnMouseDrag;
        }

        private void OnKeyPressed(object sender, KeyboardEventArgs e)
        {
            // TODO: Ajustar o deslocamento da câmera a partir do teclado (WASD e setas)
            switch (e.Key)
            {
                case Keys.W:
                    GameManager.MainCamera.Move(Vector2.UnitY * CAMERA_DRAG_SPEED * -1);
                    break;

                case Keys.A:
                    GameManager.MainCamera.Move(Vector2.UnitX * CAMERA_DRAG_SPEED * -1);
                    break;

                case Keys.S:
                    GameManager.MainCamera.Move(Vector2.UnitY * CAMERA_DRAG_SPEED);
                    break;

                case Keys.D:
                    GameManager.MainCamera.Move(Vector2.UnitX * CAMERA_DRAG_SPEED);
                    break;
            }

        }

        private void OnMouseDrag(object sender, MouseEventArgs e)
        {
            var clickScreenPos = e.Position.ToVector2();

            if (e.PreviousState.RightButton == ButtonState.Pressed)
            {
                GameManager.MainCamera.Move(e.DistanceMoved * -1);
            }

        }

        private void OnKeyReleased(object sender, KeyboardEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.F11:
                    GameManager.CurrentScene.VisualDebugMode = !GameManager.CurrentScene.VisualDebugMode;
                    break;

                case Keys.F12:
                    GameManager.CurrentSceneCompleted();
                    break;

                case Keys.R:
                    GameManager.Reset();
                    break;

                case Keys.K:
                    GameManager.CurrentScene.Player.Die();
                    break;

                case Keys.M:
                    GameManager.Sound.ToggleMute();
                    break;

                case Keys.Enter:
                    GameManager.Pause();
                    break;
            }
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            var clickScreenPos = e.Position.ToVector2();
            var destinationPos = GameManager.MainCamera.ScreenToWorld(clickScreenPos);

            if (e.Button == MouseButton.Left)
            {
                if (this.Player != null && this.Player.Enabled)
                    this.Player.MoveTo(destinationPos);
            }
        }

        private void OnMouseWheelMoved(object sender, MouseEventArgs e)
        {
            GameManager.MainCamera.Zoom += 0.2f * e.ScrollWheelDelta / Math.Abs(e.ScrollWheelDelta);
        }

        public override void Update(GameTime t)
        {
            base.Update(t);

            this.mouseListener.Update(t);
            this.keyboardListener.Update(t);
        }
    }
}
