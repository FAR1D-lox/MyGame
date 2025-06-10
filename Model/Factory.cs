using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyGame.Model.Objects.Labels;
using MyGame.Model.Objects.MapObjects;

namespace MyGame.Model
{
    public static class Factory
    {
        private static readonly Dictionary<string, (byte type, int width, int height)> _objects = new()
            {
                {"MainCharacter", ((byte) ObjectTypes.player, 34, 59) },
                {"Enemy", ((byte) ObjectTypes.enemy, 37, 64) },
                {"Grass", ((byte) ObjectTypes.grass, 120, 120) },
                {"Stone", ((byte) ObjectTypes.stone, 120, 120) },
                {"StoneNoSolid", ((byte) ObjectTypes.stoneNoSolid, 120, 120) },
                {"PlayerVerticalAttack", ((byte) ObjectTypes.playerVerticalAttack, 16, 64) },
                {"PlayerHorisontalAttack", ((byte) ObjectTypes.playerHorisontalAttack, 64, 16) },
                {"EnemyAttack", ((byte) ObjectTypes.enemyAttack, 128, 128) },
                {"LoseWindow", ((byte) ObjectTypes.loseWindow, 960, 540) },
                {"RestartButton", ((byte) ObjectTypes.restartButton, 480, 150) },
                {"ExitToMenuButton", ((byte) ObjectTypes.exitToMenuButton, 160, 160) },
                {"PauseWindow", ((byte) ObjectTypes.pauseWindow, 960, 540) },
                {"PauseButton", ((byte) ObjectTypes.pauseButton, 160, 120) },
                {"ContinueButton", ((byte) ObjectTypes.continueButton, 160, 160) }
            };
        public static MainCharacter CreateMainCharacter(float x, float y, Vector2 speed)
        {
            var player = new MainCharacter(
                new Vector2(x, y),
                _objects["MainCharacter"].width,
                _objects["MainCharacter"].height,
                 _objects["MainCharacter"].type);
            player.ChangeSpeed(speed.X, speed.Y);
            return player;
        }

        public static Enemy CreateEnemy(float x, float y, Vector2 speed)
        {
            var enemy = new Enemy(
                new Vector2(x, y),
                _objects["Enemy"].width,
                _objects["Enemy"].height,
                _objects["Enemy"].type);
            enemy.ChangeSpeed(speed.X, speed.Y);
            return enemy;
        }

        public static BlockGrass CreateGrass(float x, float y)
        {
            var grass = new BlockGrass(
                new Vector2(x, y),
                _objects["Grass"].width,
                _objects["Grass"].height,
                _objects["Grass"].type);
            return grass;
        }

        public static BlockStone CreateStone(float x, float y)
        {
            var dirt = new BlockStone(
                new Vector2(x, y),
                _objects["Stone"].width,
                _objects["Stone"].height,
                _objects["Stone"].type);
            return dirt;
        }

        public static BlockStoneNoSolid CreateStoneNoSolid(float x, float y)
        {
            var dirtNoSolid = new BlockStoneNoSolid(
                new Vector2(x, y),
                _objects["StoneNoSolid"].width,
                _objects["StoneNoSolid"].height,
                _objects["StoneNoSolid"].type);
            return dirtNoSolid;
        }

        public static PlayerVerticalAttack CreatePlayerVecticalAttack(float x, float y, Direction direction)
        {
            var playerVerticalAttack = new PlayerVerticalAttack(new Vector2(x, y),
                _objects["PlayerVerticalAttack"].width,
                _objects["PlayerVerticalAttack"].height,
                _objects["PlayerVerticalAttack"].type,
                direction);
            return playerVerticalAttack;
        }

        public static PlayerHorisontalAttack CreatePlayerHorisontalAttack(float x, float y, Direction direction)
        {
            var playerHorisontalAttack = new PlayerHorisontalAttack(
                new Vector2(x, y),
                _objects["PlayerHorisontalAttack"].width,
                _objects["PlayerHorisontalAttack"].height,
                _objects["PlayerHorisontalAttack"].type,
                direction);
            return playerHorisontalAttack;
        }

        public static EnemyAttack CreateEnemyAttack(float x, float y, Direction direction)
        {
            var enemyAttack = new EnemyAttack(
                new Vector2(x, y),
                _objects["EnemyAttack"].width,
                _objects["EnemyAttack"].height,
                _objects["EnemyAttack"].type,
                direction);
            return enemyAttack;
        }

        public static LoseWindow CreateLoseWindow(float x, float y)
        {
            var loseWindow = new LoseWindow(
                new Vector2(
                    x - _objects["LoseWindow"].width * 0.5f,
                    y - _objects["LoseWindow"].height),
                _objects["LoseWindow"].width,
                _objects["LoseWindow"].height,
                _objects["LoseWindow"].type);
            return loseWindow;
        }

        public static RestartButton CreateRestartButton(float x, float y)
        {
            var restartButton = new RestartButton(
                new Vector2(
                    x - _objects["RestartButton"].width * 0.5f,
                    y - _objects["RestartButton"].height),
                _objects["RestartButton"].width,
                _objects["RestartButton"].height,
                _objects["RestartButton"].type);
            return restartButton;
        }

        public static PauseWindow CreatePauseWindow(float x, float y)
        {
            var pauseWindow = new PauseWindow(
                new Vector2(
                    x - _objects["PauseWindow"].width * 0.5f,
                    y - _objects["PauseWindow"].height),
                _objects["PauseWindow"].width,
                _objects["PauseWindow"].height,
                _objects["PauseWindow"].type);
            return pauseWindow;
        }

        public static PauseButton CreatePauseButton(float x, float y)
        {
            var pauseButton = new PauseButton(
                new Vector2(
                    x - _objects["PauseButton"].width * 0.5f,
                    y - _objects["PauseButton"].height),
                _objects["PauseButton"].width,
                _objects["PauseButton"].height,
                _objects["PauseButton"].type);
            return pauseButton;
        }

        public static ExitToMenuButton CreateExitToMenuButton(float x, float y)
        {
            var exitToMenuButton = new ExitToMenuButton(
                new Vector2(
                    x - _objects["ExitToMenuButton"].width * 0.5f - 206,
                    y - _objects["ExitToMenuButton"].height - 79),
                _objects["ExitToMenuButton"].width,
                _objects["ExitToMenuButton"].height,
                _objects["ExitToMenuButton"].type);
            return exitToMenuButton;
        }

        public static ContinueButton CreateContinueButton(float x, float y)
        {
            var continueButton = new ContinueButton(
                new Vector2(
                    x - _objects["ContinueButton"].width * 0.5f + 206,
                    y - _objects["ContinueButton"].height - 79),
                _objects["ContinueButton"].width,
                _objects["ContinueButton"].height,
                _objects["ContinueButton"].type);
            return continueButton;
        }

        public enum ObjectTypes : byte
        {
            player,
            enemy,
            grass,
            stone,
            stoneNoSolid,
            playerVerticalAttack,
            playerHorisontalAttack,
            enemyAttack,
            loseWindow,
            restartButton,
            pauseWindow,
            pauseButton,
            exitToMenuButton,
            continueButton
        }
    }
}
