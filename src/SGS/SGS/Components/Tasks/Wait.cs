using Microsoft.Xna.Framework;
using SGS.Components.Tasks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.Components.Tasks
{
    public class Wait : ITask
    {
        private double duration;
        private double elapsed;
        private bool finished;
        private Action<GameTime> completationTask;

        public Boolean Finished
        {
            get { return this.finished; }
        }
                                
        public void Update(GameTime gameTime)
        {
            if (finished)
                return;

            this.elapsed += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (this.elapsed >= this.duration)
            {
                if (this.completationTask != null)
                    this.completationTask(gameTime);

                this.finished = true;
            }
        }

        public void Start()
        {
            this.Restart();
        }

        public void Restart()
        {
            this.elapsed = 0;
            this.finished = false;
        }

        public void Stop()
        {
            this.finished = true;
        }

        public Wait Then(Action<GameTime> completationTask)
        {
            System.Diagnostics.Debug.Assert(completationTask != null);

            this.completationTask = completationTask;

            return this;
        }

        public static Wait Milliseconds(float ms)
        {
            System.Diagnostics.Debug.Assert(ms > 0);

            var w = new Wait();
            w.elapsed = 0.0f;
            w.duration = ms;
            w.finished = true;

            return w;
        }

        public static Wait Seconds(float seconds)
        {
            return Wait.Milliseconds(seconds * 1000.0f);
        }
    }
}
