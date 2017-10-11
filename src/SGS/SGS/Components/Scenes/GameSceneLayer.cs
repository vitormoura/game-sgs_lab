using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SGS.Components.Scenes
{
    /// <summary>
    /// Representa uma camada de GameDrawableObjects, utilizada para ordenar a
    /// atualização e desenho desses componentes dentro de uma GameScene
    /// </summary>
    public class GameSceneLayer : DrawableGameObject
    {
        public Matrix ViewTransform { get; set; }

        private Effect fx;

        public override void LoadContent()
        {
            base.LoadContent();

            fx = GameManager.Content.Load<Effect>("shaders/ClosingVignettePixelShader");
        }

        public override void Draw(SpriteBatch canvas)
        {
            canvas.Begin(transformMatrix: this.ViewTransform);

            //TODO: Definir como exibir ou não efeitos
            //canvas.Begin(transformMatrix: this.ViewTransform, sortMode: SpriteSortMode.Immediate, blendState: BlendState.AlphaBlend);
            //fx.CurrentTechnique.Passes[2].Apply();
            //fx.CurrentTechnique.Passes[1].Apply();
            
            base.Draw(canvas);

            canvas.End();
        }
    }
}
