using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SGS.Components.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.Components.Sprites
{
    public class SpriteSheet : DrawableGameObject
    {
        private String assetName;
        private Texture2D texture;
        private Rectangle frameBounds;
        private SpriteSheetParams config;

        public SpriteSheet(String assetName, SpriteSheetParams param)
        {
            this.assetName = assetName;
            this.config = param;
            this.frameBounds = new Rectangle(param.bounds.X, param.bounds.Y, param.bounds.Width / param.spritesCount.X, param.bounds.Height / param.spritesCount.Y);
        }

        public override void LoadContent()
        {
            this.texture = GameManager.Content.Load<Texture2D>(this.assetName);
        }

        public Sprite GetSpriteByPos(Int32 line, Int32 col)
        {
            return this.GetSpriteByIndex(line * col);
        }

        public Sprite GetSpriteByIndex(Int32 i)
        {
            var y = config.bounds.Y +(((frameBounds.Width * i) / config.bounds.Width) * frameBounds.Height);
            var x = config.bounds.X + (((i % config.spritesCount.X)) * frameBounds.Width);
                                   
            return new Sprite(this.texture, new Rectangle(x, y, this.frameBounds.Width, this.frameBounds.Height));
        }
        
        public Sprite[] GetSpriteSequenceOfLine(Int32 lineIndex, Int32 qtd, bool backwards = false)
        {
            return this.GetSpriteSequence(lineIndex * this.config.spritesCount.X, qtd, backwards);
        }

        public Sprite[] GetSpriteSequence(Int32 startIndex, Int32 qtd, bool backwards = false)
        {
            Sprite[] sprites = new Sprite[qtd];

            for (int i = 0; i < qtd; i++)
            {
                if (!backwards)
                    sprites[i] = this.GetSpriteByIndex(startIndex + i);
                else
                    sprites[i] = this.GetSpriteByIndex(startIndex + (qtd - 1) - i);
            }

            return sprites;
        }

        public override void Draw(SpriteBatch canvas)
        {
            canvas.Draw(this.texture, this.texture.Bounds, Color.White);
        }
    }
}
