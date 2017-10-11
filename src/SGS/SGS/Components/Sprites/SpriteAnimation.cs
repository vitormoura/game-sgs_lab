using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.Components.Sprites
{
    public class SpriteAnimation : GameObject
    {
        private Action onAnimationEnd;
        private Sprite[] sprites;
        private Int32 currentFrame;
        private float elapsedSinceUpdate;
        private float timeBetweenFrames;
        private float speed;
        

        public Boolean Loop
        {
            get;
            set;
        }

        public Sprite CurrentFrame
        {
            get { return this.sprites[this.currentFrame]; }
        }

        public float Speed
        {
            get { return this.speed; }
            set
            {
                this.speed = value;
                this.timeBetweenFrames = (float)(1000.0f/value);
            }
        }

        public Sprite GetFrame(int i)
        {
            return this.sprites[i];
        }
                

        public SpriteAnimation(Sprite[] sprites, Boolean loop = true)
            : this(sprites, sprites.Length, loop )
        {
        }

        public SpriteAnimation(Sprite[] sprites, float speed, Boolean loop = true)
        {
            this.sprites = sprites;
            this.currentFrame = 0;
            this.Enabled = true;
            this.Loop = loop;
            this.Speed = speed;
        }

        public void Play()
        {
            this.Play(null);
        }

        public void Play(Action onEnd)
        {
            this.Reset();
            this.Enabled = true;
            this.onAnimationEnd = onEnd;
        }

        public void Pause()
        {
            this.Enabled = false;
        }

        public void Reset()
        {
            this.currentFrame = 0;
        }

        public override void Update(GameTime gameTime)
        {
            if(this.Enabled)
            {
                this.elapsedSinceUpdate += (float)gameTime.ElapsedGameTime.Milliseconds;

                if (elapsedSinceUpdate > this.timeBetweenFrames)
                {
                    this.currentFrame++;
                    this.elapsedSinceUpdate = 0.0f;

                    if (this.currentFrame == this.sprites.Length)
                    {
                        this.currentFrame = 0;

                        if (!this.Loop)
                        {
                            this.Enabled = false;

                            if (this.onAnimationEnd != null)
                                this.onAnimationEnd();
                        }

                        
                        
                    }
                }
            }
        }        
    }
}
