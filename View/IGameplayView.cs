using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyGame.Model;
using MyGame.Model.ObjectTypes;

namespace MyGame.View
{
    public interface IGameplayView
    {
        event EventHandler CycleFinished;
        event EventHandler<InputData> ControlInputStates;
        void LoadGameCycleParameters(Dictionary<int, IMapObject> MapObjects, Dictionary<int, ILabel> LabelObjects, Dictionary<int, IButton> ButtonObjects, Vector2 POVShift);
        void Run();
    }
}
