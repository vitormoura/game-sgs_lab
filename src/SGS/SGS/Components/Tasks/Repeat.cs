using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.Components.Tasks
{
    public class Repeat : ITask
    {
        private Action<GameTime> task;
        private Wait wait;
        private int currentRepeatCount;
        private int repeatCount;
        private bool running;

        public bool Finished
        {
            get
            {
                return !this.running;
            }
        }

        public Repeat()
        {
        }

        public void Update(GameTime gameTime)
        {
            if (!running)
                return;

            if ( repeatCount > 0 && (currentRepeatCount > repeatCount))
                this.Stop();

            this.wait.Update(gameTime);

            if (this.wait.Finished)
            {
                this.wait.Restart();
                currentRepeatCount++;
            }
        }

        public void Start()
        {
            System.Diagnostics.Debug.Assert(running == false);
            System.Diagnostics.Debug.Assert(wait != null);

            this.running = true;
            this.wait.Start();
        }

        public void Pause()
        {
            this.running = false;
        }

        public void Stop()
        {
            this.running = false;
            this.wait.Stop();
        }


        public Repeat Each(Int32 ms)
        {
            if (this.wait != null)
                this.wait.Stop();

            this.wait = Wait.Milliseconds(ms).Then(this.task);
                                    
            return this;
        }

        public Repeat Times(Int32 qtd)
        {
            System.Diagnostics.Debug.Assert(qtd > 0);
            System.Diagnostics.Debug.Assert(!running);

            this.currentRepeatCount = 0;
            this.repeatCount = qtd;

            return this;
        }

        public static Repeat Task(Action<GameTime> task)
        {
            System.Diagnostics.Debug.Assert(task != null);

            var r = new Repeat();

            r.task      = task;
            r.running   = false;
            r.wait      = null;
            r.repeatCount       = 0;
                        
            return r;
        }

        public void Restart()
        {
            throw new NotImplementedException();
        }
    }
}
