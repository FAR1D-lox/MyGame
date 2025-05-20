using MyGame.Presenter;
using System;

public static class Program
{
    [STAThread]
    static void Main()
    {  
        GameplayPresenter game = new GameplayPresenter(
          new MyGame.View.GameCycleView(), new MyGame.Model.GameCycle()
        );
        game.LaunchGame();
    }
}