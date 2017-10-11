using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using SGS.Components.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace SGS.Components.UI
{
    public class BlinkingLabel : Label
    {
        private Repeat blink;

        public BlinkingLabel(SpriteFont font) : base(font)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            this.blink = Repeat.Task(t => this.Visible = !this.Visible).Each(350);
            this.blink.Start();
        }

        public override void Update(GameTime t)
        {
            base.Update(t);

            this.blink.Update(t);
        }
    }
}
