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
using MyGame.Model.Objects.MapObjects;
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
        public event EventHandler<TimersEventArgs> UpdatedTimers;

        public int PlayerId { get; set; }
        public int PortalId { get; set; }
        public Dictionary<int, IMapObject> MapObjects { get; set; }
        public Dictionary<int, ISolidObject> SolidObjects { get; set; }
        public Dictionary<int, IGravityObject> GravityObjects { get; set; }
        public Dictionary<int, IAliveObject> AliveObjects { get; set; }
        public Dictionary<int, IAttackObject> AttackObjects { get; set; }
        public Dictionary<int, (Factory.ObjectTypes, ILabel)> LabelObjects { get; set; }
        public Dictionary<int, (Factory.ObjectTypes, IButton)> ButtonObjects { get; set; }

        private GameState GameState { get; set; }
        private Vector2 MousePosition;
        private bool IsPlayerPlaced = false;

        private int CurrentId;
        private Vector2 PlayerShift;
        private Vector2 PlayerInitPos;

        private Dictionary<string, List<int>> ButtonsId;


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

            ButtonsId = new Dictionary<string, List<int>>()
            {
                {"PauseWindow", new List<int>()},
                {"PauseButton", new List<int>()},
                {"RestartWindow", new List<int>()},
                {"Menu", new List<int>()},
                {"WinWindow", new List<int>()},
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


            var ViewLabelObjects = new Dictionary<int, ILabel>();
            foreach (var label in LabelObjects)
            {
                ViewLabelObjects.Add(label.Key, label.Value.Item2);
            }
            var ViewButtonObjects = new Dictionary<int, IButton>();
            foreach (var button in ButtonObjects)
            {
                ViewButtonObjects.Add(button.Key, button.Value.Item2);
            }

            Updated.Invoke(this, new GameplayEventArgs()
            {
                MapObjects = MapObjects,
                LabelObjects = ViewLabelObjects,
                ButtonObjects = ViewButtonObjects,
                POVShift = new Vector2(),
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
                {
                    PlayerShift = MapObjects[PlayerId].Pos - PlayerInitPos;
                    if (RectangleCollider.IsCollided(
                        (MapObjects[PlayerId] as MainCharacter).Collider,
                        (MapObjects[PortalId] as Portal).Collider)
                        && !DetectEnemies())
                    {
                        GameState = Win;
                        IsPlayerPlaced = false;
                        ButtonObjects.Clear();
                        LabelObjects.Clear();
                        ButtonsId["PauseButton"].Clear();
                        OpenWinWindow();
                    }

                }
                else
                {
                    GameState = RestartWindow;
                    IsPlayerPlaced = false;
                    ButtonObjects.Clear();
                    LabelObjects.Clear();
                    ButtonsId["PauseButton"].Clear();
                    OpenRestartWindow();
                }
            }
        }

        private bool DetectEnemies()
        {
            foreach (var obj in MapObjects.Values)
            {
                if (obj is Enemy)
                    return true;
            }
            return false;
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
            PortalId = MapCreator.PortalId;

            EnemyControl.ConnectEnemyControl(
                AliveObjects, AttackObjects, MapObjects, PlayerId);
        }

        public void ControlLabels()
        {
            if (GameState != Running)
                PlayerShift = new Vector2(0, 0);
            if (GameState != Menu)
            foreach (var button in ButtonObjects.Values)
                button.Item2.Pos = PlayerInitPos;
            
            var ViewLabelObjects = new Dictionary<int, ILabel>();
            foreach (var label in LabelObjects)
            {
                ViewLabelObjects.Add(label.Key, label.Value.Item2);
            }
            var ViewButtonObjects = new Dictionary<int, IButton>();
            foreach (var button in ButtonObjects)
            {
                ViewButtonObjects.Add(button.Key, button.Value.Item2);
            }
            Updated.Invoke(this,
                new GameplayEventArgs
                {
                    MapObjects = MapObjects,
                    LabelObjects = ViewLabelObjects,
                    ButtonObjects = ViewButtonObjects,
                    POVShift = PlayerShift,
                    GameState = GameState
                });
        }

        public void ControlMenuLabels(LabelsControlData labelsControlData)
        {
            foreach (var id in new List<int>(ButtonsId["Menu"]))
            {
                if (GameState != Menu)
                    break;
                if (ButtonObjects[id].Item1 == Factory.ObjectTypes.beginGameButton)
                    TryBeginGame(ButtonObjects[id].Item2);
                else if (ButtonObjects[id].Item1 == Factory.ObjectTypes.leaveGameButton)
                    TryLeaveGame(ButtonObjects[id].Item2);
            }
        }

        public void ControlWinLabels(LabelsControlData labelsControlData)
        {
            foreach (var id in new List<int>(ButtonsId["WinWindow"]))
            {
                if (GameState != Win)
                    break;
                if (ButtonObjects[id].Item1 == Factory.ObjectTypes.exitToMenuButton)
                    TryExitToMenu(ButtonObjects[id].Item2);
                else if (ButtonObjects[id].Item1 == Factory.ObjectTypes.restartButton2)
                    TryRestart(ButtonObjects[id].Item2);
            }
        }

        public void ControlRestartWindowLabels(LabelsControlData labelsControlData)
        {
            foreach (var id in new List<int>(ButtonsId["RestartWindow"]))
            {
                if (GameState != RestartWindow)
                    break;
                if (ButtonObjects[id].Item1 == Factory.ObjectTypes.restartButton1)
                    TryRestart(ButtonObjects[id].Item2);
            }
        }

        public void ControlPauseLabels(LabelsControlData labelsControlData)
        {
            foreach (var id in new List<int>(ButtonsId["PauseWindow"]))
            {
                if (GameState != Pause)
                    break;
                if (ButtonObjects[id].Item1 == Factory.ObjectTypes.exitToMenuButton)
                    TryExitToMenu(ButtonObjects[id].Item2);
                else if (ButtonObjects[id].Item1 == Factory.ObjectTypes.continueButton)
                    TryContinueGame(ButtonObjects[id].Item2, labelsControlData.IsEscPressed);
            }
        }

        public void ControlRunningLabels(LabelsControlData labelsControlData)
        {
            foreach (var id in new List<int>(ButtonsId["PauseButton"]))
            {
                if (GameState != Running)
                    break;
                if (ButtonObjects[id].Item1 == Factory.ObjectTypes.pauseButton)
                    TryPause(ButtonObjects[id].Item2, labelsControlData.IsEscPressed);
            }
        }


        public void UpdateTimers()
        {
            UpdatedTimers.Invoke(this,
                new TimersEventArgs()
                {
                    ButtonTimer = 10
                });
        }


        private void TryRestart(IButton button)
        {
            if (button.CursorHover)
            {
                GameState = Running;
                RestartMap();
                ButtonObjects.Clear();
                LabelObjects.Clear();
                ButtonsId["RestartWindow"].Clear();
                ButtonsId["WinWindow"].Clear();
                ShowPauseButton();
                UpdateTimers();
            }
        }

        private void TryPause(IButton button, bool isEscPressed)
        {
            if (button.CursorHover || isEscPressed)
            {
                GameState = Pause;
                ButtonObjects.Clear();
                LabelObjects.Clear();
                ButtonsId["PauseButton"].Clear();
                OpenPauseWindow();
                UpdateTimers();
            }
        }

        private void TryExitToMenu(IButton button)
        {
            if (button.CursorHover)
            {
                GameState = Menu;
                ClearMap();
                ButtonObjects.Clear();
                LabelObjects.Clear();
                ButtonsId["PauseWindow"].Clear();
                ButtonsId["WinWindow"].Clear();
                OpenMenu();
                UpdateTimers();
            }
        }

        private void TryContinueGame(IButton button, bool isEscPressed)
        {
            if (button.CursorHover || isEscPressed)
            {
                GameState = Running;
                ButtonObjects.Clear();
                LabelObjects.Clear();
                ButtonsId["PauseWindow"].Clear();
                ShowPauseButton();
                UpdateTimers();
            }
        }

        private void TryBeginGame(IButton button)
        {
            if (button.CursorHover)
            {
                GameState = Running;
                RestartMap();
                ButtonObjects.Clear();
                LabelObjects.Clear();
                ButtonsId["Menu"].Clear();
                ShowPauseButton();
                UpdateTimers();
            }
        }

        private void TryLeaveGame(IButton button)
        {
            if (button.CursorHover)
            {
                Exit.Invoke(this, new EventArgs());
                UpdateTimers();
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
            LabelObjects.Add(CurrentId, (Factory.ObjectTypes.pauseWindow, generatedLabelObject));
            CurrentId++;
            IButton generatedButtonObject;
            generatedButtonObject = Factory.CreateExitToMenuButton(PlayerInitPos.X, PlayerInitPos.Y);
            ButtonObjects.Add(CurrentId, (Factory.ObjectTypes.exitToMenuButton, generatedButtonObject));
            ButtonsId["PauseWindow"].Add(CurrentId);
            CurrentId++;
            generatedButtonObject = Factory.CreateContinueButton(PlayerInitPos.X, PlayerInitPos.Y);
            ButtonObjects.Add(CurrentId, (Factory.ObjectTypes.continueButton, generatedButtonObject));
            ButtonsId["PauseWindow"].Add(CurrentId);
            CurrentId++;
        }
        
        public void OpenRestartWindow()
        {
            ILabel generatedLabelObject;
            generatedLabelObject = Factory.CreateLoseWindow(PlayerInitPos.X, PlayerInitPos.Y);
            LabelObjects.Add(CurrentId, (Factory.ObjectTypes.loseWindow, generatedLabelObject));
            CurrentId++;
            IButton generatedButtonObject;
            generatedButtonObject = Factory.CreateRestartButton1(PlayerInitPos.X, PlayerInitPos.Y);
            ButtonObjects.Add(CurrentId, (Factory.ObjectTypes.restartButton1, generatedButtonObject));
            ButtonsId["RestartWindow"].Add(CurrentId);
            CurrentId++;
        }

        public void OpenMenu()
        {
            IButton generatedButtonObject;
            generatedButtonObject = Factory.CreateBeginGameButton(PlayerInitPos.X, PlayerInitPos.Y);
            ButtonObjects.Add(CurrentId, (Factory.ObjectTypes.beginGameButton, generatedButtonObject));
            ButtonsId["Menu"].Add(CurrentId);
            CurrentId++;
            generatedButtonObject = Factory.CreateLeaveGameButton(PlayerInitPos.X, PlayerInitPos.Y);
            ButtonObjects.Add(CurrentId, (Factory.ObjectTypes.leaveGameButton, generatedButtonObject));
            ButtonsId["Menu"].Add(CurrentId);
            CurrentId++;
        }

        public void OpenWinWindow()
        {
            ILabel generatedLabelObject;
            generatedLabelObject = Factory.CreateWinWindow(PlayerInitPos.X, PlayerInitPos.Y);
            LabelObjects.Add(CurrentId, (Factory.ObjectTypes.winWindow, generatedLabelObject));
            CurrentId++;
            IButton generatedButtonObject;
            generatedButtonObject = Factory.CreateExitToMenuButton(PlayerInitPos.X, PlayerInitPos.Y);
            ButtonObjects.Add(CurrentId, (Factory.ObjectTypes.exitToMenuButton, generatedButtonObject));
            ButtonsId["WinWindow"].Add(CurrentId);
            CurrentId++;
            generatedButtonObject = Factory.CreateRestartButton2(PlayerInitPos.X, PlayerInitPos.Y);
            ButtonObjects.Add(CurrentId, (Factory.ObjectTypes.restartButton2, generatedButtonObject));
            ButtonsId["WinWindow"].Add(CurrentId);
            CurrentId++;
        }

        public void ShowPauseButton()
        {
            IButton generatedButtonObject;
            generatedButtonObject = Factory.CreatePauseButton(PlayerInitPos.X, PlayerInitPos.Y);
            ButtonObjects.Add(CurrentId, (Factory.ObjectTypes.pauseButton, generatedButtonObject));
            ButtonsId["PauseButton"].Add(CurrentId);
            CurrentId++;
        }
    }
}