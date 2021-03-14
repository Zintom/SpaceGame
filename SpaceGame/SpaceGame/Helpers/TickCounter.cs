using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceGame.Helpers
{
    public class TickCounter
    {

        public int TicksPerSecond { get; private set; } = 0;

        private int _ticks;

        private double _lastResetTime;

        public void Tick(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.TotalMilliseconds - _lastResetTime >= 1000)
            {
                TicksPerSecond = _ticks;

                _lastResetTime = gameTime.TotalGameTime.TotalMilliseconds;
                _ticks = 0;

                return;
            }

            _ticks++;
        }

    }
}
