using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;

namespace SGS.Components.Cameras
{
    public class DefaultGameCamera : GameObject // Camera2D
    {
        private Camera2D camera2D;
        private Vector2[] movementLimitPoints;
        private RectangleF movementBoundaries;

        public float Zoom
        {
            get { return this.camera2D.Zoom; }
            set
            {
                this.camera2D.Zoom = MathHelper.Clamp(value, 0.2f, 2.0f);
            }
        }

        public Vector2 Origin
        {
            get { return this.camera2D.Origin; }
        }

        public RectangleF BoundingRectangle
        {
            get { return this.camera2D.BoundingRectangle; }
        }


        public RectangleF MovementBoundaries
        {
            get { return this.movementBoundaries; }
            set
            {
                this.movementBoundaries = value;
                this.movementLimitPoints[0] = new Vector2(value.Left, value.Top);
                this.movementLimitPoints[1] = new Vector2(value.Right - this.camera2D.BoundingRectangle.Width, value.Bottom - this.camera2D.BoundingRectangle.Height);
            }
        }

        public DefaultGameCamera(GraphicsDevice gd)
            : this(gd, null)
        {
        }

        public DefaultGameCamera(GraphicsDevice gd, RectangleF? limitBoundaries)
        {
            this.camera2D = new Camera2D(gd);
            this.movementLimitPoints = new Vector2[2];


            if (limitBoundaries != null)
            {
                this.MovementBoundaries = limitBoundaries.Value;
            }
            else
            {
                this.MovementBoundaries = new RectangleF(0, 0, gd.Viewport.Width, gd.Viewport.Height);
            }
        }

        public void Follow(GameObject target)
        {
            //TODO: revisar comportamento de follow da câmera para adequar-se a nova implementação
            /*
            if (this.FocusTarget.HasValue)
            {
                //var dir =  new Vector3(this.focusTarget.Value,0) - this.position;
                this.Position = new Vector2(MathHelper.Lerp(this.position.X, FocusTarget.Value.X, 0.5f), MathHelper.Lerp(this.position.Y, FocusTarget.Value.Y, 0.5f));


                if (Vector3.Distance(this.FocusTarget.Value, this.position) <= 1.0)
                    this.FocusTarget = null;
            }
            */
        }

        public void Reset()
        {
            this.LookAt(GameManager.GraphicsDevice.Viewport.Bounds.Center.ToVector2());
        }

        public void LookAt(Vector2 position)
        {
            this.camera2D.LookAt(position);
            ApplyBoundaries();
        }

        public Matrix GetViewMatrix()
        {
            return this.camera2D.GetViewMatrix();
        }

        public void Move(Vector2 direction)
        {
            this.camera2D.Move(direction);
            ApplyBoundaries();
        }

        public Vector2 ScreenToWorld(Vector2 screenPos)
        {
            return this.camera2D.ScreenToWorld(screenPos);
        }

        public Vector2 WorldToScreen(Vector2 worldPos)
        {
            return this.camera2D.WorldToScreen(worldPos);
        }

        private void ApplyBoundaries()
        {
            this.camera2D.Position = Vector2.Clamp(this.camera2D.Position, this.movementLimitPoints[0], this.movementLimitPoints[1]);
        }
    }
}
