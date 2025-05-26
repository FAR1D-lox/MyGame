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

namespace MyGame.Presenter
{
    public class GameplayPresenter
    {
        public IGameplayModel gameplayModel = null;
        public IGameplayView gameplayView = null;

        public GameplayPresenter(IGameplayView gameplayView, IGameplayModel gameplayModel)
        {
            this.gameplayModel = gameplayModel;
            this.gameplayView = gameplayView;

            this.gameplayModel.Updated += ModelViewUpdate;
            this.gameplayView.PlayerMoved += ViewModelMovePlayer;
            this.gameplayView.CycleFinished += ViewModelUpdate;

            this.gameplayModel.Initialize();
        }

        private void ViewModelMovePlayer(object sender, ControlsEventArgs e)
        {
            gameplayModel.ControlPlayer(e);
        }

        private void ModelViewUpdate(object sender, GameplayEventArgs e)
        {
            gameplayView.LoadGameCycleParameters(e.Objects, e.POVShift);
        }

        private void ViewModelUpdate(object sender, GameTimeEventArgs e)
        {
            gameplayModel.Update(e.GameTime);
        }

        public void LaunchGame()
        {
            gameplayView.Run();
        }
    }
}
