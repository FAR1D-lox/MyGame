using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyGame.Model;
using MyGame.Model.Objects.MapObjects;
using MyGame.Model.ObjectTypes;
using MyGame.Presenter;

namespace MyGame.View
{
    public interface IGameplayView
    {
        event EventHandler CycleFinished;
        event EventHandler<InputData> ControlInputStates;
        void LoadGameCycleParameters(
            Dictionary<int, ISolidObject> SolidObjects,
            Dictionary<int, NoSolidObject> NoSolidObjects,
            Dictionary<int, IAttackObject> AttackObjects,
            Dictionary<int, BackgroundObject> BackgroundObjects,
            Dictionary<int, IWindow> WindowObjects,
            Dictionary<int, IButton> ButtonObjects,
            Vector2 POVShift, GameState GameState);
        void Run();
        void ExitGame();
    }
}
