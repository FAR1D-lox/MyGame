using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyGame.Model.Objects.Labels;
using MyGame.Model.ObjectTypes;
using MyGame.Presenter;
using MyGame.View;
using static MyGame.Presenter.GameState;

namespace MyGame.Model
{
    public class GameCycle : IGameplayModel
    {
        public event EventHandler<GameplayEventArgs> Updated;

        public int PlayerId { get; set; }
        public Dictionary<int, IMapObject> MapObjects { get; set; }
        public Dictionary<int, ISolidObject> SolidObjects { get; set; }
        public Dictionary<int, IGravityObject> GravityObjects { get; set; }
        public Dictionary<int, IAliveObject> AliveObjects { get; set; }
        public Dictionary<int, IAttackObject> AttackObjects { get; set; }
        public Dictionary<int, ILabel> LabelObjects { get; set; }
        public Dictionary<int, IButton> ButtonObjects { get; set; }

        private GameState GameState { get; set; }

        private Vector2 MousePosition;
        private bool IsPlayerPlaced = false;

        private int CurrentId;
        private Vector2 PlayerShift;
        private Vector2 PlayerInitPos;
        private MouseClick MouseLeftButtonState;

        public void Initialize()
        {
            MapObjects = new Dictionary<int, IMapObject>();
            SolidObjects = new Dictionary<int, ISolidObject>();
            GravityObjects = new Dictionary<int, IGravityObject>();
            AliveObjects = new Dictionary<int, IAliveObject>();
            AttackObjects = new Dictionary<int, IAttackObject>();
            LabelObjects = new Dictionary<int, ILabel>();
            ButtonObjects = new Dictionary<int, IButton>();


            PlayerControl.ConnectPlayerControl(
                MapObjects, AttackObjects);
            AttacksControl.ConnectAttacksControl(
                AliveObjects, GravityObjects, SolidObjects, AttackObjects, MapObjects);
            CollisionCalculater.ConnectCollisionCalculater(
                MapObjects, SolidObjects, GravityObjects);
            MapCreator.ConnectMapCreator(
                MapObjects, SolidObjects, GravityObjects, AliveObjects);

            MapCreator.CreateFirstMap();
            CurrentId = MapCreator.CurrentId;
            PlayerId = MapCreator.PlayerId;
            IsPlayerPlaced = MapCreator.IsPlayerPlaced;

            EnemyControl.ConnectEnemyControl(
                AliveObjects, AttackObjects, MapObjects, PlayerId);

            ButtonObjects.Add(CurrentId, Factory.CreatePauseButton(PlayerInitPos.X, PlayerInitPos.Y));
            CurrentId++;
            

            Updated.Invoke(this, new GameplayEventArgs()
            {
                MapObjects = MapObjects,
                LabelObjects = LabelObjects,
                ButtonObjects = ButtonObjects,
                POVShift = new Vector2(
                    MapObjects[PlayerId].Pos.X,
                    MapObjects[PlayerId].Pos.Y),
                GameState = GameState
            });
        }

        

        public void UpdateMap()
        {
            if (IsPlayerPlaced)
            {
                PlayerInitPos = MapObjects[PlayerId].Pos;

                EnemyControl.BeginEnemyControl(CurrentId);
                CurrentId = EnemyControl.CurrentId;

                UpdateGravityObjectsSpeed();
                CollisionCalculater.ActivateCollisionCalculater();
                AttacksControl.ActivateAttacksControl();

                if (MapObjects.ContainsKey(PlayerId))
                    PlayerShift = MapObjects[PlayerId].Pos - PlayerInitPos;
                else
                {
                    GameState = RestartWindow;
                    OpenRestartWindow();
                    IsPlayerPlaced = false;
                    PlayerShift = new Vector2(0, 0);
                }
            }

            Updated.Invoke(this,
                new GameplayEventArgs
                {
                    MapObjects = MapObjects,
                    LabelObjects = LabelObjects,
                    ButtonObjects = ButtonObjects,
                    POVShift = PlayerShift,
                    GameState = GameState
                });
        }

        public void ControlLabels(LabelsControlData e)
        {
            MouseLeftButtonState = e.MouseLeftButtonState;
            MousePosition = e.MousePosition;
            foreach (var buttonId in ButtonObjects.Keys)
            {
                ButtonObjects[buttonId].CheckCursorHover(MousePosition);
                if (ButtonObjects[buttonId] is RestartButton restartButton)
                    TryRestart(restartButton);
                else if (ButtonObjects[buttonId] is PauseButton pauseButton)
                    TryPause(pauseButton, e.IsEscPressed);
                else if (ButtonObjects[buttonId] is ExitToMenuButton exitToMenuButton)
                    TryExitToMenu(exitToMenuButton);
                else if (ButtonObjects[buttonId] is ContinueButton continueButton)
                    TryContinueGame(continueButton, e.IsEscPressed);
            }
        }


        private void TryRestart(RestartButton button)
        {
            if (button.CursorHover && MouseLeftButtonState == MouseClick.pressed)
            {
                Initialize();
                PlayerShift = new Vector2(float.MinValue, float.MinValue);
                GameState = Running;
            }
        }

        private void TryPause(PauseButton button, bool isEcsPressed)
        {
            if (isEcsPressed ||
                button.CursorHover && MouseLeftButtonState == MouseClick.pressed)
            {
                GameState = Pause;
                OpenPauseWindow();
                foreach (var buttonId in ButtonObjects.Keys)
                {
                    if (ButtonObjects[buttonId] is PauseButton)
                        ButtonObjects.Remove(buttonId);
                }
            }
        }

        private void TryExitToMenu(ExitToMenuButton button)
        {
            if (button.CursorHover && MouseLeftButtonState == MouseClick.pressed)
            {
                GameState = Menu;
            }
        }

        private void TryContinueGame(ContinueButton button, bool isEcsPressed)
        {
            if (isEcsPressed ||
                button.CursorHover && MouseLeftButtonState == MouseClick.pressed)
            {
                GameState = Running;
                foreach (var buttonId in ButtonObjects.Keys)
                {
                    if (ButtonObjects[buttonId] is ContinueButton ||
                        ButtonObjects[buttonId] is ExitToMenuButton)
                        ButtonObjects.Remove(buttonId);
                }
                foreach (var labelId in LabelObjects.Keys)
                {
                    if (ButtonObjects[labelId] is PauseWindow)
                        ButtonObjects.Remove(labelId);
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

        public void ControlPlayerGameplay(MainCharacterControlData e)
        {
            if (IsPlayerPlaced)
            {
                PlayerControl.BeginPlayerControl(PlayerId, CurrentId, e);
                CurrentId = PlayerControl.CurrentId;
            }
        }



        public void OpenPauseWindow()
        {
            GameState = Pause;
            ILabel generatedLabelObject;
            generatedLabelObject = Factory.CreatePauseWindow(PlayerInitPos.X, PlayerInitPos.Y);
            LabelObjects.Add(CurrentId, generatedLabelObject);
            CurrentId++;
            IButton generatedButtonObject;
            generatedButtonObject = Factory.CreateExitToMenuButton(PlayerInitPos.X, PlayerInitPos.Y);
            ButtonObjects.Add(CurrentId, generatedButtonObject);
            CurrentId++;
            generatedButtonObject = Factory.CreateContinueButton(PlayerInitPos.X, PlayerInitPos.Y);
            ButtonObjects.Add(CurrentId, generatedButtonObject);
            CurrentId++;
        }
        
        public void OpenRestartWindow()
        {
            GameState = RestartWindow;
            ILabel generatedLabelObject;
            generatedLabelObject = Factory.CreateLoseWindow(PlayerInitPos.X, PlayerInitPos.Y);
            LabelObjects.Add(CurrentId, generatedLabelObject);
            CurrentId++;
            IButton generatedButtonObject;
            generatedButtonObject = Factory.CreateRestartButton(PlayerInitPos.X, PlayerInitPos.Y);
            ButtonObjects.Add(CurrentId, generatedButtonObject);
            CurrentId++;
        }

        public void OpenMenu()
        {
            GameState = Menu;
            
        }

        public void ContinueGame()
        {
            
        }
    }
}