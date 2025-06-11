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
        public event EventHandler<EventArgs> Exit;

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

        private Dictionary<string, List<int>> LabelsId;

        private int EscTimer = 0;
        private int ButtonTimer = 0;

        public void Initialize()
        {
            GameState = Menu;

            MapObjects = new();
            SolidObjects = new();
            GravityObjects = new();
            AliveObjects = new();
            AttackObjects = new();

            LabelObjects = new();
            ButtonObjects = new();

            LabelsId = new Dictionary<string, List<int>>()
            {
                {"PauseWindow", new List<int>()},
                {"PauseButton", new List<int>()},
                {"RestartWindow", new List<int>()},
                {"Menu", new List<int>()},
            };

            OpenMenu();

            PlayerControl.ConnectPlayerControl(
                MapObjects, AttackObjects);
            AttacksControl.ConnectAttacksControl(
                AliveObjects, GravityObjects, SolidObjects, AttackObjects, MapObjects);
            CollisionCalculater.ConnectCollisionCalculater(
                MapObjects, SolidObjects, GravityObjects);
            MapCreator.ConnectMapCreator(
                MapObjects, SolidObjects, GravityObjects, AliveObjects);

            


            Updated.Invoke(this, new GameplayEventArgs()
            {
                MapObjects = MapObjects,
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
                    IsPlayerPlaced = false;
                    OpenRestartWindow();
                }
            }
        }

        private void RestartMap()
        {
            ClearMap();
            StartMap();
        }

        private void ClearMap()
        {
            MapObjects.Clear();
            SolidObjects.Clear();
            GravityObjects.Clear();
            AliveObjects.Clear();
            AttackObjects.Clear();
        }

        private void StartMap()
        {
            MapCreator.CreateFirstMap();
            CurrentId = MapCreator.CurrentId;
            PlayerId = MapCreator.PlayerId;
            IsPlayerPlaced = MapCreator.IsPlayerPlaced;

            EnemyControl.ConnectEnemyControl(
                AliveObjects, AttackObjects, MapObjects, PlayerId);
            ShowPauseButton();
        }

        public void ControlLabels(LabelsControlData e)
        {
            if (GameState != Running)
                PlayerShift = new Vector2(0, 0);
            foreach (var button in ButtonObjects.Values)
                if (!(button is BeginGameButton || button is LeaveGameButton))
                    button.Pos = PlayerInitPos;

            MouseLeftButtonState = e.MouseLeftButtonState;
            MousePosition = e.MousePosition;

            foreach (var button in new Dictionary<int, IButton>(ButtonObjects).Values)
            {
                button.CheckCursorHover(MousePosition);
                if (button is RestartButton restartButton)
                    TryRestart(restartButton);
                else if (button is PauseButton pauseButton)
                    TryPause(pauseButton, e.IsEscPressed);
                else if (button is ExitToMenuButton exitToMenuButton)
                    TryExitToMenu(exitToMenuButton);
                else if (button is ContinueButton continueButton)
                    TryContinueGame(continueButton, e.IsEscPressed);
                else if (button is BeginGameButton beginGameButton)
                    TryBeginGame(beginGameButton);
                else if (button is LeaveGameButton leaveGameButton)
                    TryLeaveGame(leaveGameButton);

            }
            EscTimer -= 1;
            ButtonTimer -= 1;
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

        private void TryRestart(RestartButton button)
        {
            if (button.CursorHover && MouseLeftButtonState == MouseClick.pressed && ButtonTimer <= 0)
            {
                RestartMap();
                PlayerShift = new Vector2(float.MinValue, float.MinValue);
                GameState = Running;
                foreach (var id in LabelsId["RestartWindow"])
                {
                    ButtonObjects.Remove(id);
                    LabelObjects.Remove(id);
                }
            }
        }

        private void TryPause(PauseButton button, bool isEscPressed)
        {
            if ((isEscPressed && EscTimer <= 0) ||
                button.CursorHover && MouseLeftButtonState == MouseClick.pressed && ButtonTimer <= 0)
            {
                GameState = Pause;
                OpenPauseWindow();
                foreach (var id in LabelsId["PauseButton"])
                    ButtonObjects.Remove(id);
                EscTimer = 10;
                ButtonTimer = 10;
            }
        }

        private void TryExitToMenu(ExitToMenuButton button)
        {
            if (button.CursorHover && MouseLeftButtonState == MouseClick.pressed && ButtonTimer <= 0)
            {
                GameState = Menu;
                ClearMap();
                foreach (var id in LabelsId["PauseWindow"])
                {
                    ButtonObjects.Remove(id);
                    LabelObjects.Remove(id);
                }
                OpenMenu();
                ButtonTimer = 10;
            }
        }

        private void TryContinueGame(ContinueButton button, bool isEscPressed)
        {
            if ((isEscPressed && EscTimer <= 0) ||
                button.CursorHover && MouseLeftButtonState == MouseClick.pressed && ButtonTimer <= 0)
            {
                GameState = Running;
                foreach (var id in LabelsId["PauseWindow"])
                {
                    ButtonObjects.Remove(id);
                    LabelObjects.Remove(id);
                }
                ShowPauseButton();
                EscTimer = 10;
                ButtonTimer = 10;
            }
        }

        private void TryBeginGame(BeginGameButton button)
        {
            if (button.CursorHover && MouseLeftButtonState == MouseClick.pressed && ButtonTimer <= 0)
            {
                GameState = Running;
                foreach (var id in LabelsId["Menu"])
                {
                    ButtonObjects.Remove(id);
                }
                RestartMap();
                ShowPauseButton();
                ButtonTimer = 10;
            }
        }

        private void TryLeaveGame(LeaveGameButton button)
        {
            if (button.CursorHover && MouseLeftButtonState == MouseClick.pressed && ButtonTimer <= 0)
            {
                Exit.Invoke(this, new EventArgs());
                ButtonTimer = 10;
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
            ILabel generatedLabelObject;
            generatedLabelObject = Factory.CreatePauseWindow(PlayerInitPos.X, PlayerInitPos.Y);
            LabelObjects.Add(CurrentId, generatedLabelObject);
            LabelsId["PauseWindow"].Add(CurrentId);
            CurrentId++;
            IButton generatedButtonObject;
            generatedButtonObject = Factory.CreateExitToMenuButton(PlayerInitPos.X, PlayerInitPos.Y);
            ButtonObjects.Add(CurrentId, generatedButtonObject);
            LabelsId["PauseWindow"].Add(CurrentId);
            CurrentId++;
            generatedButtonObject = Factory.CreateContinueButton(PlayerInitPos.X, PlayerInitPos.Y);
            ButtonObjects.Add(CurrentId, generatedButtonObject);
            LabelsId["PauseWindow"].Add(CurrentId);
            CurrentId++;
        }
        
        public void OpenRestartWindow()
        {
            ILabel generatedLabelObject;
            generatedLabelObject = Factory.CreateLoseWindow(PlayerInitPos.X, PlayerInitPos.Y);
            LabelObjects.Add(CurrentId, generatedLabelObject);
            LabelsId["RestartWindow"].Add(CurrentId);
            CurrentId++;
            IButton generatedButtonObject;
            generatedButtonObject = Factory.CreateRestartButton(PlayerInitPos.X, PlayerInitPos.Y);
            ButtonObjects.Add(CurrentId, generatedButtonObject);
            LabelsId["RestartWindow"].Add(CurrentId);
            CurrentId++;
        }

        public void OpenMenu()
        {
            IButton generatedButtonObject;
            generatedButtonObject = Factory.CreateBeginGameButton(PlayerInitPos.X, PlayerInitPos.Y);
            ButtonObjects.Add(CurrentId, generatedButtonObject);
            LabelsId["Menu"].Add(CurrentId);
            CurrentId++;
            generatedButtonObject = Factory.CreateLeaveGameButton(PlayerInitPos.X, PlayerInitPos.Y);
            ButtonObjects.Add(CurrentId, generatedButtonObject);
            LabelsId["Menu"].Add(CurrentId);
            CurrentId++;
        }

        public void ShowPauseButton()
        {
            IButton generatedButtonObject;
            generatedButtonObject = Factory.CreatePauseButton(PlayerInitPos.X, PlayerInitPos.Y);
            ButtonObjects.Add(CurrentId, generatedButtonObject);
            LabelsId["PauseButton"].Add(CurrentId);
            CurrentId++;
        }

        public void ContinueGame()
        {
            
        }
    }
}