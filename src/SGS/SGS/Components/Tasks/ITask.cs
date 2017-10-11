using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.Components.Tasks
{
    public interface ITask
    {
        Boolean Finished { get; }

        void Start();

        void Restart();

        void Stop();

        void Update(GameTime gt);
    }
}
