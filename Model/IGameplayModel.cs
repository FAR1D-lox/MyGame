using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyGame.Model.ObjectTypes;
using MyGame.Presenter;
using MyGame.View;

namespace MyGame.Model
{
    public interface IGameplayModel
    {
        int PlayerId { get; set; }
        Dictionary<int, IMapObject> MapObjects { get; set; }
        public Dictionary<int, ISolidObject> SolidObjects { get; set; }
        public Dictionary<int, IGravityObject> GravityObjects { get; set; }
        public Dictionary<int, IAliveObject> AliveObjects { get; set; }
        public Dictionary<int, IAttackObject> AttackObjects { get; set; }
        event EventHandler<GameplayEventArgs> Updated;
        event EventHandler<EventArgs> Exit;
        event EventHandler<TimersEventArgs> UpdatedTimers;
        void UpdateMap();

        public void ControlLabels();

        public void ControlMenuLabels(LabelsControlData labelsControlData);

        public void ControlWinLabels(LabelsControlData labelsControlData);

        public void ControlRestartWindowLabels(LabelsControlData labelsControlData);

        public void ControlPauseLabels(LabelsControlData labelsControlData);

        public void ControlRunningLabels(LabelsControlData labelsControlData);
        void ControlPlayerGameplay(MainCharacterControlData e);
        void Initialize();
    }

    public class GameplayEventArgs : EventArgs
    {
        public Dictionary<int, IMapObject> MapObjects { get; set; }
        public Dictionary<int, ILabel> LabelObjects { get; set; }
        public Dictionary<int, IButton> ButtonObjects { get; set; }
        public Vector2 POVShift { get; set; }
        public GameState GameState { get; set; }
    }

    public class TimersEventArgs : EventArgs
    {
        public int ButtonTimer;
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
