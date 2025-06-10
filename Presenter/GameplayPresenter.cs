using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyGame.Model;
using MyGame.View;
using static MyGame.Presenter.GameState;

namespace MyGame.Presenter
{
    public class GameplayPresenter
    {
        public IGameplayModel gameplayModel = null;
        public IGameplayView gameplayView = null;
        GameState GameState { get; set; }

        public GameplayPresenter(IGameplayView gameplayView, IGameplayModel gameplayModel)
        {
            this.gameplayModel = gameplayModel;
            this.gameplayView = gameplayView;

            this.gameplayModel.Updated += ModelViewUpdate;
            this.gameplayView.ControlInputStates += ViewModelReadInput;
            this.gameplayView.CycleFinished += ViewModelUpdate;

            this.gameplayModel.Initialize();
        }

        private void ViewModelReadInput(object sender, InputData e)
        {
            if (GameState == Running)
            {
                gameplayModel.ControlPlayerGameplay(
                    new MainCharacterControlData
                    {
                        Direction = Controller.FindDirection(e.PressedKeys),
                        MouseLeftButtonState = Controller.MouseController(e.MouseLeftButtonState)
                    }
                    );

            }

            else if (GameState == RestartWindow)
            {
                
            }
            else if (GameState == Pause)
            {
                
            }
            else if (GameState == Menu)
            {
                
            }

            gameplayModel.ControlLabels(
                new LabelsControlData
                {
                    MouseLeftButtonState = Controller.MouseController(e.MouseLeftButtonState),
                    MousePosition = e.MousePosition,
                    IsEscPressed = Controller.IsPressedESC(e.PressedKeys)
                }
                );
        }

        private void ModelViewUpdate(object sender, GameplayEventArgs e)
        {
            GameState = e.GameState;
            gameplayView.LoadGameCycleParameters(e.MapObjects, e.LabelObjects, e.ButtonObjects, e.POVShift);
        }

        private void ViewModelUpdate(object sender, EventArgs e)
        {
            if (GameState == Running)
                gameplayModel.UpdateMap();

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
        RestartWindow
    }
}
