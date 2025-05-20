using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Model
{
    public class GameTimeEventArgs : EventArgs
    {
        public GameTime GameTime { get; }

        public GameTimeEventArgs(GameTime gameTime)
        {
            GameTime = gameTime;
        }
    }
}
