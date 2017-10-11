using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SGS.Components.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.Components
{
    /// <summary>
    /// Representa um objeto de jogo capaz de ser desenhado em uma 
    /// camada de desenho padrão
    /// </summary>
    public abstract class DrawableGameObject : GameObject
    {
        protected List<DrawableGameObject> drawables;

        /// <summary>
        /// Posição onde o objeto será desenhado
        /// </summary>
        public virtual Vector2 Position { get; set; }

        /// <summary>
        /// Ponto central do objeto de desenho
        /// </summary>
        public Vector2 Center
        {
            get
            {
                return this.Bounds.Center.ToVector2();
            }

            set
            {
                this.Position = new Vector2(value.X - (this.Bounds.Width / 2), value.Y - (this.Bounds.Height / 2));
            }
        }

        /// <summary>
        /// Limites do objeto desenhado
        /// </summary>
        public Rectangle Bounds
        {
            get;
            protected set;
        }

        /// <summary>
        /// Determina se o objeto deve ou não ser desenhado
        /// </summary>
        public bool Visible
        {
            get;
            set;
        }

        public DrawableGameObject()
        {
            this.Visible = true;
            this.drawables = new List<DrawableGameObject>();
        }

        /// <summary>
        /// Implementações devem realizar a operação de desenho propriamente dita
        /// </summary>
        /// <param name="layer"></param>
        public virtual void Draw(SpriteBatch canvas)
        {
            foreach (var c in this.drawables)
            {
                if (c.Visible)
                    c.Draw(canvas);
            }
        }

        /// <summary>
        /// Adiciona um novo GameObject associado a esse objeto
        /// </summary>
        /// <param name="c"></param>
        public override void AddChild(GameObject c)
        {
            if(c is DrawableGameObject)
            {
                var drawable = c as DrawableGameObject;
                this.drawables.Add(drawable);
            }

            base.AddChild(c);
        }

        public override void Unload()
        {
            base.Unload();
            this.drawables.Clear();
        }

        public override void Enable()
        {
            base.Enable();
            this.Visible = true;
        }

        public override void Disable()
        {
            base.Disable();
            this.Visible = false;
        }
    }
}
