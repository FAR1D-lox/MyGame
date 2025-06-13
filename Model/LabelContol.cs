using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using MyGame.View;
using MonoGame.Framework.Utilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using static MyGame.Model.Direction;
using static Microsoft.Xna.Framework.Input.ButtonState;
using MyGame.Presenter;
using MyGame.Model.ObjectTypes;
using MyGame.Model.Objects.MapObjects;

namespace MyGame.Model
{
    public static class LabelContol
    {
        private static Dictionary<int, (Factory.ObjectTypes, IWindow)> WindowObjects;
        private static Dictionary<int, (Factory.ObjectTypes, IButton)> ButtonObjects;
        private static int CurrentId;
        private static Dictionary<string, List<int>> ButtonsId;
        public static void ConnectLabelControl(
            Dictionary<int, (Factory.ObjectTypes, IWindow)> windowObjects,
            Dictionary<int, (Factory.ObjectTypes, IButton)> buttonObjects,
            Dictionary<string, List<int>> buttonsId)
        {
            WindowObjects = windowObjects;
            ButtonObjects = buttonObjects;
            ButtonsId = buttonsId;
        }

        public static void OpenPauseWindow(Vector2 PlayerInitPos, int currentId)
        {
            CurrentId = currentId;
            IWindow generatedLabelObject;
            generatedLabelObject = Factory.CreatePauseWindow(PlayerInitPos.X, PlayerInitPos.Y);
            WindowObjects.Add(CurrentId, (Factory.ObjectTypes.pauseWindow, generatedLabelObject));
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

        public static void OpenRestartWindow(Vector2 PlayerInitPos, int currentId)
        {
            CurrentId = currentId;
            IWindow generatedLabelObject;
            generatedLabelObject = Factory.CreateLoseWindow(PlayerInitPos.X, PlayerInitPos.Y);
            WindowObjects.Add(CurrentId, (Factory.ObjectTypes.loseWindow, generatedLabelObject));
            CurrentId++;
            IButton generatedButtonObject;
            generatedButtonObject = Factory.CreateRestartButton1(PlayerInitPos.X, PlayerInitPos.Y);
            ButtonObjects.Add(CurrentId, (Factory.ObjectTypes.restartButton1, generatedButtonObject));
            ButtonsId["RestartWindow"].Add(CurrentId);
            CurrentId++;
        }

        public static void OpenMenu(Vector2 PlayerInitPos, int currentId)
        {
            CurrentId = currentId;
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

        public static void OpenWinWindow(Vector2 PlayerInitPos, int currentId)
        {
            CurrentId = currentId;
            IWindow generatedLabelObject;
            generatedLabelObject = Factory.CreateWinWindow(PlayerInitPos.X, PlayerInitPos.Y);
            WindowObjects.Add(CurrentId, (Factory.ObjectTypes.winWindow, generatedLabelObject));
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

        public static void ShowPauseButton(Vector2 PlayerInitPos, int currentId)
        {
            CurrentId = currentId;
            IButton generatedButtonObject;
            generatedButtonObject = Factory.CreatePauseButton(PlayerInitPos.X, PlayerInitPos.Y);
            ButtonObjects.Add(CurrentId, (Factory.ObjectTypes.pauseButton, generatedButtonObject));
            ButtonsId["PauseButton"].Add(CurrentId);
            CurrentId++;
        }
    }
}
