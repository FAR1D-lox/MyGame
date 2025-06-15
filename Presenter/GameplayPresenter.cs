using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using KnightLegends.Model;
using KnightLegends.View;
using static KnightLegends.Presenter.GameState;
using static Microsoft.Xna.Framework.Input.ButtonState;

namespace KnightLegends.Presenter
{
    public class GameplayPresenter
    {
        public IGameplayModel gameplayModel = null;
        public IGameplayView gameplayView = null;
        private GameState GameState { get; set; }
        private int ButtonTimer { get; set; }
        LabelsControlData DataToControlLabels { get; set; }

        public GameplayPresenter(IGameplayView gameplayView, IGameplayModel gameplayModel)
        {
            this.gameplayModel = gameplayModel;
            this.gameplayView = gameplayView;

            this.gameplayModel.Exit += ExitGame;
            this.gameplayModel.Updated += ModelViewUpdate;
            this.gameplayView.ControlInputStates += ViewModelReadInput;
            this.gameplayView.CycleFinished += ViewModelUpdate;
            this.gameplayModel.UpdatedTimers += UpdateTimers;

            this.gameplayModel.Initialize();
        }

        private void ViewModelReadInput(object sender, InputData e)
        {
            DataToControlLabels = new LabelsControlData
            {
                MouseLeftButtonState = e.MouseLeftButtonState,
                MousePosition = e.MousePosition,
                IsEscPressed = Controller.IsPressedESC(e.PressedKeys)
            };

            if (GameState == Running)
            {
                gameplayModel.ControlPlayerGameplay(
                    new MainCharacterControlData
                    {
                        Direction = Controller.FindDirection(e.PressedKeys),
                        MouseLeftButtonState = e.MouseLeftButtonState
                    }
                    );
            }

            if (ButtonTimer <= 0)
            {
                if (e.MouseLeftButtonState == Pressed  ||
                    Controller.IsPressedESC(e.PressedKeys))
                {
                    if (GameState == Running)
                    {
                        gameplayModel.ControlRunningLabels(DataToControlLabels);
                    }
                    else if (GameState == Pause)
                    {
                        gameplayModel.ControlPauseLabels(DataToControlLabels);
                    }
                }
                if (e.MouseLeftButtonState == Pressed)
                {
                    if (GameState == RestartWindow)
                    {
                        gameplayModel.ControlRestartWindowLabels(DataToControlLabels);
                    }
                    else if (GameState == Menu)
                    {
                        gameplayModel.ControlMenuLabels(DataToControlLabels);
                    }
                    else if (GameState == Win)
                    {
                        gameplayModel.ControlWinLabels(DataToControlLabels);
                    }
                }
            }

            gameplayModel.ControlLabels();
            ButtonTimer -= 1;
        }

        private void ModelViewUpdate(object sender, GameplayEventArgs e)
        {
            GameState = e.GameState;
            gameplayView.LoadGameCycleParameters(
                e.SolidObjects, e.NoSolidObjects, e.AttackObjects, e.BackgroundObjects, e.WindowObjects, e.ButtonObjects, e.POVShift, e.GameState);
        }

        private void ViewModelUpdate(object sender, EventArgs e)
        {
            if (GameState == Running)
                gameplayModel.UpdateMap();

        }

        private void UpdateTimers(object sender, TimersEventArgs e)
        {
            ButtonTimer = e.ButtonTimer;
        }

        private void ExitGame(object sender, EventArgs e)
        {
            gameplayView.ExitGame();
        }

        public void LaunchGame()
        {
            gameplayView.Run();
        }

    }

    public enum GameState
    {
        Running,
        Menu,
        Pause,
        RestartWindow,
        Win
    }
}
