using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.Components.UI
{
    public class FancyLabel : DrawableGameObject
    {
        private Texture2D fontAtlas;
        private Dictionary<string, Rectangle> fontAtlasMap;
        private float avgWidth;
        private float maxWidth;
        private float maxHeight;
        private Vector2 currentPosition;
        private Vector2 positionOffset;
        private string character;

        public String Text { get; set; }
        public Color? Color { get; set; }
        public float Scale { get; set; }
        public float Spacing { get; set; }
        public float Rotation { get; set; }

        public FancyLabel()
        {
            this.Text = String.Empty;
            this.Scale = 0.8f;
            this.Spacing = 0f;
            this.Rotation = 0.0f;
            //this.Color = Color.SkyBlue;
        }

        public override void LoadContent()
        {
            fontAtlas = GameManager.Content.Load<Texture2D>("sprites/fancy_font");
            fontAtlasMap = GameManager.Content.Load<Dictionary<string, Rectangle>>("sprites/fancy_font_map");
        }

        public override void Initialize()
        {
            base.Initialize();

            maxWidth = (float)fontAtlasMap.Values.Max(x => x.Width);
            maxHeight = (float)fontAtlasMap.Values.Max(x => x.Height);
            avgWidth = (float)fontAtlasMap.Values.Average(x => x.Width);
            positionOffset = new Vector2(maxWidth / 2, maxHeight / 2);
        }

        public override void Draw(SpriteBatch canvas)
        {
            StripTextAccentuation();

            currentPosition = Position + positionOffset;
            var i = 0;
            while (i < Text.Length)
            {
                character = Text[i].ToString().ToUpper();

                if (!fontAtlasMap.ContainsKey(character))
                {
                    if (Text[i] == '\n')
                        currentPosition = new Vector2(Position.X + positionOffset.X, currentPosition.Y + maxHeight);
                    else
                        currentPosition.X += avgWidth / 2;

                    i++;
                    continue;
                }

                float auxRotation = 0;
                if (Rotation != 0)
                    auxRotation = i % 2 == 0 ? Rotation : -Rotation;

                canvas.Draw(fontAtlas, position: currentPosition, sourceRectangle: fontAtlasMap[character], color: Color,
                    origin: new Vector2(fontAtlasMap[character].Width / 2, fontAtlasMap[character].Height / 2),
                    scale: Vector2.One * Scale, rotation: auxRotation);

                currentPosition.X += ((fontAtlasMap[character].Width > avgWidth ? fontAtlasMap[character].Width : avgWidth) + Spacing) * Scale;

                i++;
            }
        }

        private void StripTextAccentuation()
        {
            string accentedStr = Text;
            byte[] tempBytes;
            tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(accentedStr);
            Text = System.Text.Encoding.UTF8.GetString(tempBytes);
        }
    }
}
