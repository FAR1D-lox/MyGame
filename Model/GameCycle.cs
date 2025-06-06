using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyGame.View;

namespace MyGame.Model
{
    public class GameCycle : IGameplayModel
    {
        public event EventHandler<GameplayEventArgs> Updated;

        public int PlayerId { get; set; }
        public Dictionary<int, IObject> Objects { get; set; }
        public Dictionary<int, IMapObject> MapObjects { get; set; }
        public Dictionary<int, ISolidObject> SolidObjects { get; set; }
        public Dictionary<int, IGravityObject> GravityObjects { get; set; }
        public Dictionary<int, IAliveObject> AliveObjects { get; set; }
        public Dictionary<int, IAttackObject> AttackObjects { get; set; }
        public Dictionary<int, ILabel> LabelObjects { get; set; }
        public Dictionary<int, IButton> ButtonObjects { get; set; }

        private Vector2 MousePosition;
        private bool IsPlayerPlaced = false;

        private int CurrentId;
        private Vector2 PlayerShift;
        private MouseClick MouseLeftButtonState;

        public void Initialize()
        {
            Objects = new Dictionary<int, IObject>();
            MapObjects = new Dictionary<int, IMapObject>();
            SolidObjects = new Dictionary<int, ISolidObject>();
            GravityObjects = new Dictionary<int, IGravityObject>();
            AliveObjects = new Dictionary<int, IAliveObject>();
            AttackObjects = new Dictionary<int, IAttackObject>();
            LabelObjects = new Dictionary<int, ILabel>();
            ButtonObjects = new Dictionary<int, IButton>();


            PlayerControl.ConnectPlayerControl(
                Objects, MapObjects, AttackObjects);
            AttacksControl.ConnectAttacksControl(
                Objects, AliveObjects, GravityObjects, SolidObjects, AttackObjects, MapObjects);
            CollisionCalculater.ConnectCollisionCalculater(
                Objects, MapObjects, SolidObjects, GravityObjects);
            MapCreator.ConnectMapCreator(
                Objects, MapObjects, SolidObjects, GravityObjects, AliveObjects);

            MapCreator.CreateFirstMap();
            CurrentId = MapCreator.CurrentId;
            PlayerId = MapCreator.PlayerId;
            IsPlayerPlaced = MapCreator.IsPlayerPlaced;

            EnemyControl.ConnectEnemyControl(Objects, AliveObjects, AttackObjects, MapObjects, PlayerId);
            
            Updated.Invoke(this, new GameplayEventArgs()
            {
                Objects = Objects,
                POVShift = new Vector2(
                    Objects[PlayerId].Pos.X,
                    Objects[PlayerId].Pos.Y)
            });
        }

        

        public void Update(GameTime gameTime)
        {
            if (IsPlayerPlaced)
            {
                Vector2 playerInitPos = MapObjects[PlayerId].Pos;

                EnemyControl.BeginEnemyControl(CurrentId);
                CurrentId = EnemyControl.CurrentId;

                UpdateGravityObjectsSpeed();
                CollisionCalculater.ActivateCollisionCalculater();
                AttacksControl.ActivateAttacksControl();

                if (MapObjects.ContainsKey(PlayerId))
                    PlayerShift = MapObjects[PlayerId].Pos - playerInitPos;
                else
                {
                    OpenWindow(playerInitPos);
                    IsPlayerPlaced = false;
                    PlayerShift = new Vector2(0, 0);
                }
            }

            TryRestart();

            Updated.Invoke(this, new GameplayEventArgs
            {
                Objects = Objects,
                POVShift = PlayerShift
            });
        }

        private void OpenWindow(Vector2 playerInitPos)
        {
            ILabel generatedLabelObject;
            generatedLabelObject = Factory.CreateLoseWindow(playerInitPos.X, playerInitPos.Y);
            Objects.Add(CurrentId, generatedLabelObject);
            LabelObjects.Add(CurrentId, generatedLabelObject);
            CurrentId++;
            IButton generatedButtonObject;
            generatedButtonObject = Factory.CreateRestartButton(playerInitPos.X, playerInitPos.Y);
            Objects.Add(CurrentId, generatedButtonObject);
            LabelObjects.Add(CurrentId, generatedButtonObject);
            ButtonObjects.Add(CurrentId, generatedButtonObject);
            CurrentId++;
        }

        private void TryRestart()
        {
            foreach (var button in ButtonObjects.Values)
            {
                button.CheckCursorHover(MousePosition);
                if (button.CursorHover && MouseLeftButtonState == MouseClick.pressed)
                {
                    Initialize();
                    PlayerShift = new Vector2(float.MinValue, float.MinValue);
                }
            }
        }

        private void UpdateGravityObjectsSpeed()
        {
            foreach (var gravityObject in GravityObjects.Values)
            {
                gravityObject.UpdateGravity();
            }
        }

        public void ControlPlayerGameplay(ControlsEventArgs e)
        {
            if (IsPlayerPlaced)
            {
                PlayerControl.BeginPlayerControl(PlayerId, CurrentId, e);
                PlayerId = PlayerControl.PlayerId;
                CurrentId = PlayerControl.CurrentId;
            }
            MousePosition = e.MousePosition;
            MouseLeftButtonState = e.MouseLeftButtonState;
        }


    }
}