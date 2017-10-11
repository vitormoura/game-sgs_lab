using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.Components.Sprites
{
    public class SpriteBatchLayer : DrawableGameObject
    {
        public Matrix ViewTransform { get; set; }
        
        public override void Draw(SpriteBatch canvas)
        {
            canvas.Begin(transformMatrix: this.ViewTransform);

            base.Draw(canvas);

            canvas.End();

            base.Draw(canvas);
        }
    }
}
