using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyGame.Model;

namespace MyGame.View
{
    public interface IGameplayView
    {
        event EventHandler<GameTimeEventArgs> CycleFinished;
        event EventHandler<ControlsEventArgs> PlayerMoved;
        void LoadGameCycleParameters(Dictionary<int, IObject> Objects, Vector2 POVShift);
        void Run();
    }

    public class ControlsEventArgs : EventArgs
    {
        public IGameplayModel.Direction direction { get; set; }
        public IGameplayModel.MouseClick MouseLeftBottomState { get; set; }
    }

}
