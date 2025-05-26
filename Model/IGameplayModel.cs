using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyGame.View;

namespace MyGame.Model
{
    public interface IGameplayModel
    {
        int PlayerId { get; set; }
        Dictionary<int, IObject> Objects { get; set; }
        event EventHandler<GameplayEventArgs> Updated;
        void Update(GameTime gameTime);
        void ControlPlayer(ControlsEventArgs e);
        void Initialize();
        
        public enum MouseClick : byte
        {
            released,
            pressed
        }

        public enum Direction : byte
        {
            left,
            right,
            up,
            down,
            leftUp,
            rightUp,
            None
        }
    }

    public class GameplayEventArgs : EventArgs
    {
        public Dictionary<int, IObject> Objects { get; set; }
        public Vector2 POVShift { get; set; }
    }
}
