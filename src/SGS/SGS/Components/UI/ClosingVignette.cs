using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SGS.Components.UI
{
    public class ClosingVignette : DrawableGameObject 
    {
        public event EventHandler OnFinished;

        private Effect effect;
        private Texture2D texture;
        private float closing_proportion;
        
        public Color Color
        {
            get;
            set;
        }

        public float FadeSpeed
        {
            get;
            set;
        }

        public Boolean Finished
        {
            get { return this.closing_proportion == 0.0f; }
        }
                
        public override void Initialize()
        {
            base.Initialize();

            this.Position = new Vector2(GameManager.GraphicsDevice.Viewport.Width / 2, GameManager.GraphicsDevice.Viewport.Height / 2);
            this.Bounds = new Rectangle(0, 0, GameManager.GraphicsDevice.Viewport.Width, GameManager.GraphicsDevice.Viewport.Height);
                        
            closing_proportion = 1.0f;

            this.FadeSpeed = 1.0f;
            this.Color = Color.White;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            this.texture    = GameManager.Content.Load<Texture2D>("sprites/bomb"); //DUMMY SPRITE
            this.effect     = GameManager.Content.Load<Effect>("shaders/ClosingVignettePixelShader");
        }

        public override void Update(GameTime t)
        {
            base.Update(t);

            closing_proportion -= this.FadeSpeed * (float)t.ElapsedGameTime.TotalSeconds;

            if (closing_proportion <= 0.0f)
            {
                closing_proportion = 0.0f;
                                
                if (this.OnFinished != null)
                    this.OnFinished(this, null);
            }
        }

        public override void Draw(SpriteBatch canvas)
        {
            base.Draw(canvas);

            canvas.End();

            canvas.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            effect.Parameters["CircleProportion"].SetValue(closing_proportion);
            effect.Parameters["Color"].SetValue(this.Color.ToVector4());

            effect.CurrentTechnique.Passes[0].Apply();
                        
            canvas.Draw(texture, new Rectangle((int)this.Position.X - (int)this.Bounds.Width / 2,
                                            (int)this.Position.Y - (int)this.Bounds.Height / 2,
                                            (int)this.Bounds.Width,
                                            (int)this.Bounds.Height), this.Color);
            
            canvas.End();
        }
    }
}
