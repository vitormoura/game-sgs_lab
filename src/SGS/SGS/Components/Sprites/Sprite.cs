using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using SGS.Components.Scenes;

namespace SGS.Components.Sprites
{
    public class Sprite : DrawableGameObject
    {
        private     Texture2D   texture;
        private     String      assetName;
        public Vector2 Scale { get; set; }


        public SpriteEffects Effects { get; set; }

        public Color Color { get; set; }

        public Sprite(String assetName)
            : this(assetName, Rectangle.Empty, Vector2.Zero)
        {
        }

        public Sprite(String assetName, Vector2 initialPos)
            : this(assetName, Rectangle.Empty, initialPos)
        {
        }

        public Sprite(Texture2D tx2D, Rectangle sourceRectangle)
            : this(tx2D, Vector2.Zero, sourceRectangle)
        {
        }
                
        public Sprite(Texture2D tx2D, Vector2 initialPos, Rectangle sourceRectangle)
            : this(tx2D.Name, sourceRectangle, initialPos)
        {
            this.texture = tx2D;
            this.Effects = SpriteEffects.None;
        }

        private Sprite(String assetName, Rectangle sourceRectangle, Vector2 initialPos)
        {
            this.assetName = assetName;
            this.Position = initialPos;
            this.Scale = Vector2.One;
            this.Bounds = sourceRectangle;
            this.Color = Color.White;
        }

        public override void LoadContent()
        {
            if (this.texture == null)
            {
                this.texture = GameManager.Content.Load<Texture2D>(this.assetName);
            }

            if (this.Bounds == Rectangle.Empty)
                this.Bounds = this.texture.Bounds;
        }

        public override void Draw(SpriteBatch canvas)
        {
            canvas.Draw(this.texture, 
                position: this.Position, 
                sourceRectangle: this.Bounds, 
                color:  this.Color, 
                scale: this.Scale, 
                effects: this.Effects
           );
        }
    }
}
