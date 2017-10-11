using SGS.Components.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SGS.Components.Players
{
    public class PlayerMind : DrawableGameObject
    {
        private Player player;
        private SpeechBubble speechBuddle;
        private bool bootstrapMessageShowed;
        private float totalTimeNotMoving;
        private float totalRespawns;
        private float totalLastSequenceOfDeaths;
                
        public PlayerMind(Player p)
            : base()
        {
            this.player = p;
            this.speechBuddle = new SpeechBubble();
            this.player.PlayerDied += OnPlayerDie;
            this.player.PlayerRespawn += OnPlayerRespawn;
        }

        private void OnPlayerRespawn(object sender, EventArgs e)
        {
            this.speechBuddle.Visible = false;
            this.totalRespawns++;
        }

        private void OnPlayerDie(object sender, EventArgs e)
        {
            this.speechBuddle.Visible = false;
            this.totalLastSequenceOfDeaths++;
        }

        public override void LoadContent()
        {
            this.AddChild(this.speechBuddle);

            base.LoadContent();
        }

        public override void Update(GameTime t)
        {
            if(this.totalRespawns == 1 && !this.bootstrapMessageShowed)
            {
                if (!this.speechBuddle.Visible)
                {
                    this.speechBuddle.Say(SpeechBubble.Options.Simbora);
                    this.bootstrapMessageShowed = true;
                }
            }

            if (this.totalLastSequenceOfDeaths > 0 && this.totalLastSequenceOfDeaths % 10 == 0)
            {
                if (!this.speechBuddle.Visible)
                {
                    this.speechBuddle.Say(SpeechBubble.Options.PerdendoAsContas);
                    this.totalLastSequenceOfDeaths = 0;
                }
            }

            if (!this.player.State.Moving)
                totalTimeNotMoving += (float)t.ElapsedGameTime.TotalSeconds;
            else
                totalTimeNotMoving = 0;

            if (!this.player.State.Moving && totalTimeNotMoving > 10)
            {
                if(!this.speechBuddle.Visible)
                    this.speechBuddle.Say(SpeechBubble.Options.NaoTenhoDiaTodo);
            }

            this.speechBuddle.Position = new Vector2(this.player.BoundingBox.Right - this.player.Bounds.Width / 2, this.player.Bounds.Top - this.player.Bounds.Height / 2);

            base.Update(t);
        }        
    }
}
