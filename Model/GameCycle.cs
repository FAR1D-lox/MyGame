using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using KnightLegends.Model.Objects.Labels;
using KnightLegends.Model.Objects.MapObjects;
using KnightLegends.Model.ObjectTypes;
using KnightLegends.Presenter;
using KnightLegends.View;
using static KnightLegends.Presenter.GameState;

namespace KnightLegends.Model
{
    public class GameCycle : IGameplayModel
    {
        public event EventHandler<GameplayEventArgs> Updated;
        public event EventHandler<EventArgs> Exit;
        public event EventHandler<TimersEventArgs> UpdatedTimers;

        public int PlayerId { get; set; }
        public int PortalId { get; set; }
        public int CloudsId { get; set; }
        public Dictionary<int, IMapObject> MapObjects { get; set; }
        public Dictionary<int, ISolidObject> SolidObjects { get; set; }
        public Dictionary<int, NoSolidObject> NoSolidObjects { get; set; }
        public Dictionary<int, IGravityObject> GravityObjects { get; set; }
        public Dictionary<int, IAliveObject> AliveObjects { get; set; }
        public Dictionary<int, IAttackObject> AttackObjects { get; set; }
        public Dictionary<int, BackgroundObject> BackgroundObjects { get; set; }
        public Dictionary<int, (Factory.ObjectTypes, IWindow)> WindowObjects { get; set; }
        public Dictionary<int, (Factory.ObjectTypes, IButton)> ButtonObjects { get; set; }

        private GameState GameState { get; set; }
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
            NoSolidObjects = new();
            GravityObjects = new();
            AliveObjects = new();
            AttackObjects = new();
            BackgroundObjects = new();

            WindowObjects = new();
            ButtonObjects = new();

            ButtonsId = new Dictionary<string, List<int>>()
            {
                {"PauseWindow", new List<int>()},
                {"PauseButton", new List<int>()},
                {"RestartWindow", new List<int>()},
                {"Menu", new List<int>()},
                {"WinWindow", new List<int>()},
            };

            PlayerControl.ConnectPlayerControl(
                MapObjects, AttackObjects);
            AttacksControl.ConnectAttacksControl(
                AliveObjects, GravityObjects, SolidObjects, AttackObjects, MapObjects);
            CollisionCalculater.ConnectCollisionCalculater(
                MapObjects, SolidObjects, GravityObjects);
            MapCreator.ConnectMapCreator(
                MapObjects, SolidObjects, NoSolidObjects, GravityObjects, AliveObjects, BackgroundObjects);
            LabelContol.ConnectLabelControl(WindowObjects, ButtonObjects, ButtonsId);
            LabelContol.OpenMenu(PlayerInitPos, CurrentId);

            CallUpdated();
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
            NoSolidObjects.Clear();
            GravityObjects.Clear();
            AliveObjects.Clear();
            AttackObjects.Clear();
            BackgroundObjects.Clear();
        }

        private void StartMap()
        {
            MapCreator.CreateFirstMap();
            CurrentId = MapCreator.CurrentId;
            PlayerId = MapCreator.PlayerId;
            IsPlayerPlaced = MapCreator.IsPlayerPlaced;
            PortalId = MapCreator.PortalId;
            CloudsId = MapCreator.CloudsId;

            EnemyControl.ConnectEnemyControl(
                AliveObjects, AttackObjects, MapObjects, PlayerId);
        }

        public void UpdateMap()
        {
            if (IsPlayerPlaced)
            {
                PlayerInitPos = MapObjects[PlayerId].Pos;

                EnemyControl.BeginEnemyControl(CurrentId);
                CurrentId = EnemyControl.CurrentId;

                
                CollisionCalculater.ActivateCollisionCalculater();
                AttacksControl.ActivateAttacksControl();


                if (MapObjects.ContainsKey(PlayerId))
                {
                    PlayerShift = MapObjects[PlayerId].Pos - PlayerInitPos;
                    foreach (var obj in BackgroundObjects.Values)
                    {
                        obj.Move(PlayerShift.X, PlayerShift.Y);
                    }
                    if (BackgroundObjects[CloudsId].Pos.X > MapObjects[PlayerId].Pos.X - 960)
                        BackgroundObjects[CloudsId].Move(-3000, 0);
                    else
                        BackgroundObjects[CloudsId].Move(1, 0);

                    if (RectangleCollider.IsCollided(
                        (MapObjects[PlayerId] as MainCharacter).Collider,
                        (MapObjects[PortalId] as Portal).Collider)
                        && !DetectEnemies())
                    {
                        IsPlayerPlaced = false;
                        UpdateGameStateAndLabels(Win, "PauseButton");
                        LabelContol.OpenWinWindow(PlayerInitPos, CurrentId);
                    }

                }
                else
                {
                    IsPlayerPlaced = false;
                    UpdateGameStateAndLabels(RestartWindow, "PauseButton");
                    LabelContol.OpenRestartWindow(PlayerInitPos, CurrentId);
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

        public void ControlLabels()
        {
            if (GameState != Running)
                PlayerShift = new Vector2(0, 0);
            if (GameState != Menu)
                foreach (var button in ButtonObjects.Values)
                    button.Item2.Pos = PlayerInitPos;
            CallUpdated();
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

        private void TryRestart(IButton button)
        {
            if (button.CursorHover)
            {
                UpdateGameStateAndLabels(Running, "RestartWindow", "WinWindow");
                RestartMap();
                LabelContol.ShowPauseButton(PlayerInitPos, CurrentId);
            }
        }

        private void TryPause(IButton button, bool isEscPressed)
        {
            if (button.CursorHover || isEscPressed)
            {
                UpdateGameStateAndLabels(Pause, "PauseButton");
                LabelContol.OpenPauseWindow(PlayerInitPos, CurrentId);
            }
        }

        private void TryExitToMenu(IButton button)
        {
            if (button.CursorHover)
            {
                UpdateGameStateAndLabels(Menu, "PauseWindow", "WinWindow");
                ClearMap();
                LabelContol.OpenMenu(PlayerInitPos, CurrentId);
            }
        }

        private void TryContinueGame(IButton button, bool isEscPressed)
        {
            if (button.CursorHover || isEscPressed)
            {
                UpdateGameStateAndLabels(Running, "PauseWindow");
                LabelContol.ShowPauseButton(PlayerInitPos, CurrentId);
            }
        }

        private void TryBeginGame(IButton button)
        {
            if (button.CursorHover)
            {
                UpdateGameStateAndLabels(Running, "Menu");
                RestartMap();
                LabelContol.ShowPauseButton(PlayerInitPos, CurrentId);
            }
        }

        private void TryLeaveGame(IButton button)
        {
            if (button.CursorHover)
            {
                Exit.Invoke(this, new EventArgs());
            }
        }

        private void UpdateGameStateAndLabels(GameState newGameState, string firstOpenWindow, string secondOpenWindow = null)
        {
            GameState = newGameState;
            ButtonObjects.Clear();
            WindowObjects.Clear();
            ButtonsId[firstOpenWindow].Clear();
            if (secondOpenWindow != null)
                ButtonsId[secondOpenWindow].Clear();
            UpdateTimers();
        }

        public void UpdateTimers()
        {
            UpdatedTimers.Invoke(this,
                new TimersEventArgs()
                {
                    ButtonTimer = 20
                });
        }



        public void ControlPlayerGameplay(MainCharacterControlData e)
        {
            if (IsPlayerPlaced)
            {
                PlayerControl.BeginPlayerControl(PlayerId, CurrentId, e);
                CurrentId = PlayerControl.CurrentId;
            }
        }

        public void CallUpdated()
        {
            var ViewWindowObjects = new Dictionary<int, IWindow>();
            foreach (var window in WindowObjects)
            {
                ViewWindowObjects.Add(window.Key, window.Value.Item2);
            }
            var ViewButtonObjects = new Dictionary<int, IButton>();
            foreach (var button in ButtonObjects)
            {
                ViewButtonObjects.Add(button.Key, button.Value.Item2);
            }
            Updated.Invoke(this,
                new GameplayEventArgs
                {
                    NoSolidObjects = NoSolidObjects,
                    SolidObjects = SolidObjects,
                    AttackObjects = AttackObjects,
                    BackgroundObjects = BackgroundObjects,
                    WindowObjects = ViewWindowObjects,
                    ButtonObjects = ViewButtonObjects,
                    POVShift = PlayerShift,
                    GameState = GameState
                });
        }
    }
}