using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using SGS.Components.Players;

namespace SGS.Components
{
    /// <summary>
    /// Representação de um objeto de jogo atualizável durante execução do gameloop
    /// </summary>
    public abstract class GameObject : MonoGame.Extended.IUpdate
    {

        private List<GameObject> children;

        /// <summary>
        /// GameObject 'dono' deste objeto
        /// </summary>
        public GameObject Parent { get; private set; }

        /// <summary>
        /// Identificador da categoria ou tipo do GameObject
        /// </summary>
        public Object Tag
        {
            get;
            set;
        }

        /// <summary>
        /// Determina se o gameobject está habilitado para novas atualizações
        /// </summary>
        public virtual bool Enabled
        {
            get;
            set;
        }

        public GameObject()
        {
            this.Enabled = true;
            this.children = new List<GameObject>();
        }

        /// <summary>
        /// Executa rotinas de inicialização antes do primeiro Update
        /// </summary>
        public virtual void Initialize()
        {
            this.Enabled = true;

            foreach (var c in this.children)
                c.Initialize();
        }

        /// <summary>
        /// Implementações deverão sobrescrever esse método quando desejarem carregar
        /// conteúdos utilizando um ContentManager
        /// </summary>
        /// <param name="content"></param>
        public virtual void LoadContent()
        {
            foreach (var c in this.children)
                c.LoadContent();
        }

        /// <summary>
        /// Libera recursos
        /// </summary>
        public virtual void Unload()
        {
            foreach (var c in this.children)
                c.Unload();

            this.children.Clear();
        }

        /// <summary>
        /// Quando redefinido, objetos de jogo deve realizar rotinas de atualização
        /// do seu estado interno
        /// </summary>
        /// <param name="t"></param>
        public virtual void Update(GameTime t)
        {
            if (this.Enabled)
            {
                for (int i = 0; i < this.children.Count; i++)
                {
                    if (this.children[i].Enabled)
                        this.children[i].Update(t);
                }
            }

        }

        public virtual void Disable()
        {
            this.Enabled = false;
        }

        public virtual void Enable()
        {
            this.Enabled = true;
        }

        public virtual void AddChild(GameObject c)
        {
            c.Parent = this;

            this.children.Add(c);
        }

        public void RemoveChildByIndex(Int32 index)
        {
            this.children.RemoveAt(index);
        }

        public void AddChildren(IEnumerable<GameObject> components)
        {
            foreach (var c in components)
                this.AddChild(c);
        }

        public void AddChildren(params GameObject[] components)
        {
            this.AddChildren((IEnumerable<GameObject>)components);
        }

        protected void ClearChildren()
        {
            this.children.Clear();
        }
    }
}
