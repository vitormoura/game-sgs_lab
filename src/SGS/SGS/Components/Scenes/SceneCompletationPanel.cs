using SGS.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SGS.Components.Tasks;
using SGS.Components.UI;

namespace SGS.Components.Scenes
{
    public class SceneCompletationPanel : StaticTextureFullscreenPanel
    {
        private Wait sceneClose;
                
        public SceneCompletationPanel(String message)
            : base("sprites/screen_trynext_dark")
        {
            this.Message.Text = "";
        }

        public override void Initialize()
        {
            this.sceneClose = Wait.Milliseconds(3000).Then(t =>
            {
                this.Disable();
                GameManager.PlayNextScene();
            });
        }

        public override void Update(GameTime t)
        {
            base.Update(t);
            this.sceneClose.Update(t);
        }
                
        public void Run()
        {
            this.Enable();
            this.sceneClose.Start();
        }
    }
}
