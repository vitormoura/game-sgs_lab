using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.Components.UI
{
    public class ScrollingLabel : Label
    {
        public enum Direction { UP, RIGHT, DOWN, LEFT }
        public Direction ScrollDirection { get; set; }
        public float ScrollSpeed { get; set; }
        private bool isScrolling;

        public ScrollingLabel(SpriteFont font)
            : base(font)
        {
            this.ScrollDirection = Direction.UP;
            this.ScrollSpeed = 10f;
            this.isScrolling = true;
        }

        public override void Update(GameTime t)
        {
            if (isScrolling)
            {
                // TODO: Respeitar o tamanho da janela quando redimensionada.
                switch (ScrollDirection)
                {
                    case Direction.UP:
                        Position -= new Vector2(0, (float)t.ElapsedGameTime.TotalSeconds * ScrollSpeed);
                        break;

                    case Direction.RIGHT:
                        Position += new Vector2((float)t.ElapsedGameTime.TotalSeconds * ScrollSpeed, 0);
                        break;

                    case Direction.DOWN:
                        Position += new Vector2(0, (float)t.ElapsedGameTime.TotalSeconds * ScrollSpeed);
                        break;

                    case Direction.LEFT:
                        Position -= new Vector2((float)t.ElapsedGameTime.TotalSeconds * ScrollSpeed, 0);
                        break;
                }
            }

            base.Update(t);
        }

        public void StopScrolling()
        {
            isScrolling = false;
        }
    }
}
