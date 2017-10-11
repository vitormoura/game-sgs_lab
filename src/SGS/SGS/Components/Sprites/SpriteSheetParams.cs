using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.Components.Sprites
{
    public class SpriteSheetParams
    {
        public Rectangle bounds;
        public Point spritesCount;
                
        public SpriteSheetParams(Int32 x, Int32 y, Int32 width, Int32 height, Int32 qtdSpritesPerLine, Int32 qtdSpritesPerCol)
        {
            this.bounds = new Rectangle(x, y, width, height);
            this.spritesCount = new Point(qtdSpritesPerLine, qtdSpritesPerCol);
        }
    }
}
