using KnightLegends.Presenter;
using System;

public static class Program
{
    [STAThread]
    static void Main()
    {  
        GameplayPresenter game = new GameplayPresenter(
          new KnightLegends.View.GameCycleView(), new KnightLegends.Model.GameCycle()
        );
        game.LaunchGame();
    }
}