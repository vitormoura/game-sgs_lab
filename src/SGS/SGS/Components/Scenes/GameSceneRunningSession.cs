using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.Components.Scenes
{
    /// <summary>
    /// Registra informações sobre uma sessão de execução de uma GameScene
    /// </summary>
    public class GameSceneRunningSession
    {
        private float? elapsedPausedSeconds;
        private DateTime? startPause;

        /// <summary>
        /// Início da sessão de execução
        /// </summary>
        public DateTime Start { get; private set; }

        /// <summary>
        /// Término da sessão de execução
        /// </summary>
        public DateTime? End { get; private set; }

        /// <summary>
        /// Tempo em execução
        /// </summary>
        public TimeSpan? Running { get; private set; }

        /// <summary>
        /// Tempo em pausa
        /// </summary>
        public TimeSpan? Paused { get; private set; }

        public void Begin()
        {
            this.Start = DateTime.Now;
            this.End = null;
            this.startPause = null;
            this.elapsedPausedSeconds = null;
        }

        public void Finish()
        {
            if (this.End != null)
                return;

            this.End = DateTime.Now;
            this.Running = this.End.Value - this.Start;
            
            if (this.elapsedPausedSeconds.HasValue)
            {
                this.Paused = TimeSpan.FromSeconds(this.elapsedPausedSeconds.Value);
            }
            else
                this.Paused = TimeSpan.Zero;

            this.Running = (this.End.Value - this.Start) - this.Paused;
        }

        public void Pause()
        {
            this.startPause = DateTime.Now;
        }

        public void Resume()
        {
            if (this.startPause == null)
                return;

            if (!this.elapsedPausedSeconds.HasValue)
                this.elapsedPausedSeconds = 0.0f;

            this.elapsedPausedSeconds += (float)(DateTime.Now - this.startPause.Value).TotalSeconds;
            this.startPause = null;
        }
    }
}
