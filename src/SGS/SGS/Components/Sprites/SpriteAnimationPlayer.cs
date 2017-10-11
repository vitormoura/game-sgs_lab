using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SGS.Components.Scenes;
using Microsoft.Xna.Framework.Graphics;

namespace SGS.Components.Sprites
{
    public class SpriteAnimationPlayer : DrawableGameObject
    {
        public SpriteAnimation CurrentScene { get; private set; }
                
        public SpriteAnimationPlayer()
        {
        }

        public override void Update(GameTime t)
        {
            if (this.CurrentScene != null && this.CurrentScene.Enabled)
                this.CurrentScene.Update(t);
        }

        public override void Draw(SpriteBatch canvas)
        {
            if (this.CurrentScene != null && this.CurrentScene.Enabled)
            {
                var bounds = ((DrawableGameObject)this.Parent).Bounds;

                this.CurrentScene.CurrentFrame.Position = new Vector2(bounds.X, bounds.Y);
                this.CurrentScene.CurrentFrame.Draw(canvas);
            }
        }

        public void Play(SpriteAnimation animation, Action onEndAnimation = null)
        {
            if (this.CurrentScene != null && this.CurrentScene != animation)
                this.CurrentScene.Disable();
                        
            if(this.CurrentScene != animation)
            {
                this.CurrentScene = animation;
                this.CurrentScene.Play(onEnd: onEndAnimation);
            }
        }

        public void Pause()
        {
            if (this.CurrentScene != null)
                this.CurrentScene.Pause();
        }

        public void Reset()
        {
            if (this.CurrentScene != null)
                this.CurrentScene.Reset();
        }
    }
}
