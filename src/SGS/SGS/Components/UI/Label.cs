using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.Components.UI
{
    public class Label : DrawableGameObject
    {
        private Vector2 destinationRectangleSize;
        private Rectangle destinationRectangle;
        public Rectangle DestinationRectangle { get { return destinationRectangle; } }
        private SpriteFont font;

        public String Text { get; set; }
        public Color Color { get; set; }
        
        public Alignment AlignmentFlags { get; set; }
        
        [Flags]
        public enum Alignment { Center = 1, Left = 2, Right = 4, Top = 8, Bottom = 16, VerticalCenter = 32 }

        public class Fonts
        {
            public const string DEFAULT = "fonts/default";
            public const string CREDITS = "fonts/credits";
            public const string CREDITS_ENDING = "fonts/credits_ending";
        }

        public Label(SpriteFont font)
        {
            this.font = font;
            this.Text = String.Empty;
            this.Color = Color.White;
            this.AlignmentFlags = Alignment.Left | Alignment.Top;
        }

        public override void Initialize()
        {
            base.Initialize();

            destinationRectangleSize = font.MeasureString(Text);
            destinationRectangle = new Rectangle(Position.ToPoint(), destinationRectangleSize.ToPoint());
        }

        public override void Update(GameTime t)
        {
            destinationRectangleSize = font.MeasureString(Text);
            destinationRectangle = new Rectangle(Position.ToPoint(), destinationRectangleSize.ToPoint());

            base.Update(t);
        }

        public override void Draw(SpriteBatch canvas)
        {
            if (destinationRectangle != null)
            {
/*#if DEBUG
                canvas.DrawRectangle(new RectangleF(destinationRectangle), Color.Orange);
                canvas.DrawPoint(destinationRectangle.Center.ToVector2(), Color.Orange, 2);
#endif*/
                DrawString(canvas, font, destinationRectangle);
            }
        }

        private void DrawString(SpriteBatch canvas, SpriteFont font, Rectangle bounds)
        {
            String[] lines = Text.Split('\n');

            var i = 0;
            while (i < lines.Length)
            {
                var size = font.MeasureString(lines[i]);
                Vector2 origin = size / 2;

                if (AlignmentFlags.HasFlag(Alignment.Left))
                    origin.X += (bounds.Width - size.X) / 2;

                if (AlignmentFlags.HasFlag(Alignment.Right))
                    origin.X -= (bounds.Width - size.X) / 2;

                origin.Y = (size.Y * ((lines.Length / 2.0f) - i));

                if (AlignmentFlags.HasFlag(Alignment.Top))
                    origin.Y = (bounds.Height / 2 ) - (size.Y * i);

                if (AlignmentFlags.HasFlag(Alignment.Bottom))
                    origin.Y = ((bounds.Height / 2) - (size.Y * (lines.Length - i))) * -1;

                canvas.DrawString(font, lines[i++], bounds.Center.ToVector2(), Color, 0, origin, 1, SpriteEffects.None, 0);
            }
        }
    }
}
