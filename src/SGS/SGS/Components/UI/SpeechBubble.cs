using Microsoft.Xna.Framework;
using SGS.Components.Sprites;
using SGS.Components.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace SGS.Components.UI
{
    public class SpeechBubble : DrawableGameObject
    {
        public enum Options
        {
            Simbora,
            NaoTenhoDiaTodo,
            PerdendoAsContas
        }
                
        private Dictionary<Options, Sprite[]> messages;
        private Wait hideMessage;
        private Options currentMessage;
        private int currentSubMessage;
        private Random random;
                
        public SpeechBubble()
        {
        }

        public override void Initialize()
        {
            this.random = new Random();
            this.hideMessage = Wait.Seconds(2).Then(Hide);
            this.hideMessage.Start();
        }

        public override void LoadContent()
        {
            this.messages = new Dictionary<Options, Sprite[]>();
                        
            this.messages.Add(Options.Simbora,new Sprite[] { new Sprite("sprites/msg_001"), new Sprite("sprites/msg_001.1"), new Sprite("sprites/msg_001.2") });
            this.messages.Add(Options.NaoTenhoDiaTodo, new Sprite[] { new Sprite("sprites/msg_002"), new Sprite("sprites/msg_002.1"), new Sprite("sprites/msg_002.2") });
            this.messages.Add(Options.PerdendoAsContas, new Sprite[] { new Sprite("sprites/msg_003"), new Sprite("sprites/msg_003.1"), new Sprite("sprites/msg_003.2") });

            foreach (var s in this.messages.Values)
            {
                for (int i = 0; i < s.Length; i++)
                {
                    s[i].LoadContent();
                }
            }
        }

        public override void Update(GameTime t)
        {
            this.hideMessage.Update(t);
        }

        public override void Draw(SpriteBatch canvas)
        {
            this.messages[this.currentMessage][this.currentSubMessage].Position = this.Position;
            this.messages[this.currentMessage][this.currentSubMessage].Draw(canvas);
        }

        public void Say(Options o)
        {
            this.Visible = true;

            this.currentMessage = o;
            this.currentSubMessage = this.random.Next(this.messages[0].Length);

            this.hideMessage.Restart();
        }

        public void Hide(GameTime t)
        {
            this.Visible = false;
        }
    }
}
