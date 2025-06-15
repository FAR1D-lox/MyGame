using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using KnightLegends.Model.Objects.Labels;
using KnightLegends.Model.Objects.MapObjects;

namespace KnightLegends.Model
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
                {"Portal", ((byte) ObjectTypes.portal, 71, 89) },
                {"LoseWindow", ((byte) ObjectTypes.loseWindow, 960, 540) },
                {"RestartButton1", ((byte) ObjectTypes.restartButton1, 480, 150) },
                {"ExitToMenuButton", ((byte) ObjectTypes.exitToMenuButton, 160, 160) },
                {"PauseWindow", ((byte) ObjectTypes.pauseWindow, 960, 540) },
                {"PauseButton", ((byte) ObjectTypes.pauseButton, 160, 120) },
                {"ContinueButton", ((byte) ObjectTypes.continueButton, 160, 160) },
                {"BeginGameButton", ((byte) ObjectTypes.beginGameButton, 480, 150) },
                {"LeaveGameButton", ((byte) ObjectTypes.leaveGameButton, 480, 150) },
                {"WinWindow", ((byte) ObjectTypes.winWindow, 960, 540) },
                {"RestartButton2", ((byte) ObjectTypes.restartButton2, 160, 160) },
                {"Clouds", ((byte) ObjectTypes.clouds, 6000, 400) },
                {"Sun", ((byte) ObjectTypes.sun, 200, 440) },
                {"Mountains", ((byte) ObjectTypes.mountains, 2000, 680) },
                {"JapanHouse", ((byte) ObjectTypes.japanHouse, 240, 360) },
                {"Sakura", ((byte) ObjectTypes.sakura, 360, 360) },
                {"Tree", ((byte) ObjectTypes.tree, 120, 240) },
                {"DeathTable", ((byte) ObjectTypes.deathTable, 120, 120) },
                {"AngryEnemy", ((byte) ObjectTypes.angryEnemy, 37, 64) },
                {"AngryEnemyAttack", ((byte) ObjectTypes.angryEnemyAttack, 128, 128) }
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
                _objects["Enemy"].type,
                ObjectTypes.enemy,
                2f);
            enemy.ChangeSpeed(speed.X, speed.Y);
            return enemy;
        }

        public static Block CreateGrass(float x, float y)
        {
            var grass = new Block(
                new Vector2(x, y),
                _objects["Grass"].width,
                _objects["Grass"].height,
                _objects["Grass"].type);
            return grass;
        }

        public static Block CreateStone(float x, float y)
        {
            var stone = new Block(
                new Vector2(x, y),
                _objects["Stone"].width,
                _objects["Stone"].height,
                _objects["Stone"].type);
            return stone;
        }

        public static NoSolidObject CreateStoneNoSolid(float x, float y)
        {
            var stoneNoSolid = new NoSolidObject(
                new Vector2(x, y),
                _objects["StoneNoSolid"].width,
                _objects["StoneNoSolid"].height,
                _objects["StoneNoSolid"].type);
            return stoneNoSolid;
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
                direction,
                damage: 5);
            return enemyAttack;
        }

        public static Window CreateLoseWindow(float x, float y)
        {
            var loseWindow = new Window(
                new Vector2(
                    x - _objects["LoseWindow"].width * 0.5f,
                    y - _objects["LoseWindow"].height),
                _objects["LoseWindow"].width,
                _objects["LoseWindow"].height,
                _objects["LoseWindow"].type);
            return loseWindow;
        }

        public static Button CreateRestartButton1(float x, float y)
        {
            var restartButton = new Button(
                new Vector2(x, y),
                _objects["RestartButton1"].width,
                _objects["RestartButton1"].height,
                _objects["RestartButton1"].type,
                new Vector2(
                    -_objects["RestartButton1"].width * 0.5f,
                    -_objects["RestartButton1"].height - 50
                ));
            return restartButton;
        }

        public static Window CreatePauseWindow(float x, float y)
        {
            var pauseWindow = new Window(
                new Vector2(
                    x - _objects["PauseWindow"].width * 0.5f,
                    y - _objects["PauseWindow"].height),
                _objects["PauseWindow"].width,
                _objects["PauseWindow"].height,
                _objects["PauseWindow"].type);
            return pauseWindow;
        }

        public static Button CreatePauseButton(float x, float y)
        {
            var pauseButton = new Button(
                new Vector2(x, y),
                _objects["PauseButton"].width,
                _objects["PauseButton"].height,
                _objects["PauseButton"].type,
                new Vector2(-900, -700));
            return pauseButton;
        }

        public static Button CreateExitToMenuButton(float x, float y)
        {
            var exitToMenuButton = new Button(
                new Vector2(x, y),
                _objects["ExitToMenuButton"].width,
                _objects["ExitToMenuButton"].height,
                _objects["ExitToMenuButton"].type,
                new Vector2(-286, -238));
            return exitToMenuButton;
        }

        public static Button CreateContinueButton(float x, float y)
        {
            var continueButton = new Button(
                new Vector2(x, y),
                _objects["ContinueButton"].width,
                _objects["ContinueButton"].height,
                _objects["ContinueButton"].type,
                new Vector2(126, -238));
            return continueButton;
        }

        public static Button CreateBeginGameButton(float x, float y)
        {
            var beginGameButton = new Button(
                new Vector2(
                    x - _objects["BeginGameButton"].width / 2,
                    y - _objects["BeginGameButton"].height / 2),
                _objects["BeginGameButton"].width,
                _objects["BeginGameButton"].height,
                _objects["BeginGameButton"].type,
                new Vector2(0, -100));
            return beginGameButton;
        }

        public static Button CreateLeaveGameButton(float x, float y)
        {
            var leaveGameButton = new Button(
                new Vector2(
                    x - _objects["LeaveGameButton"].width / 2,
                    y - _objects["LeaveGameButton"].height / 2),
                _objects["LeaveGameButton"].width,
                _objects["LeaveGameButton"].height,
                _objects["LeaveGameButton"].type,
                new Vector2(0, 100));
            return leaveGameButton;
        }

        public static Portal CreatePortal(float x, float y)
        {
            var portal = new Portal(
                new Vector2(
                    x + _objects["Portal"].width / 2,
                    y + 31),
                _objects["Portal"].width,
                _objects["Portal"].height,
                _objects["Portal"].type);
            return portal;
        }

        public static Window CreateWinWindow(float x, float y)
        {
            var winWindow = new Window(
                new Vector2(
                    x - _objects["WinWindow"].width * 0.5f,
                    y - _objects["WinWindow"].height),
                _objects["WinWindow"].width,
                _objects["WinWindow"].height,
                _objects["WinWindow"].type);
            return winWindow;
        }

        public static Button CreateRestartButton2(float x, float y)
        {
            var restartButton = new Button(
                new Vector2(x, y),
                _objects["RestartButton2"].width,
                _objects["RestartButton2"].height,
                _objects["RestartButton2"].type,
                new Vector2(126, -238));
            return restartButton;
        }

        public static BackgroundObject CreateClouds(float x, float y)
        {
            var clouds = new BackgroundObject(
                new Vector2(x, y),
                _objects["Clouds"].width,
                _objects["Clouds"].height,
                _objects["Clouds"].type);
            return clouds;
        }

        public static BackgroundObject CreateSun(float x, float y)
        {
            var sun = new BackgroundObject(
                new Vector2(x, y),
                _objects["Sun"].width,
                _objects["Sun"].height,
                _objects["Sun"].type);
            return sun;
        }

        public static BackgroundObject CreateMountains(float x, float y)
        {
            var mountains = new BackgroundObject(
                new Vector2(x - 400, y + 680),
                _objects["Mountains"].width,
                _objects["Mountains"].height,
                _objects["Mountains"].type);
            return mountains;
        }

        public static NoSolidObject CreateJapanHouse(float x, float y)
        {
            var japanHouse = new NoSolidObject(
                new Vector2(x, y),
                _objects["JapanHouse"].width,
                _objects["JapanHouse"].height,
                _objects["JapanHouse"].type);
            return japanHouse;
        }

        public static NoSolidObject CreateSakura(float x, float y)
        {
            var sakura = new NoSolidObject(
                new Vector2(x, y),
                _objects["Sakura"].width,
                _objects["Sakura"].height,
                _objects["Sakura"].type);
            return sakura;
        }

        public static NoSolidObject CreateTree(float x, float y)
        {
            var tree = new NoSolidObject(
                new Vector2(x, y),
                _objects["Tree"].width,
                _objects["Tree"].height,
                _objects["Tree"].type);
            return tree;
        }

        public static NoSolidObject CreateDeathTable(float x, float y)
        {
            var deathTable = new NoSolidObject(
                new Vector2(x, y),
                _objects["DeathTable"].width,
                _objects["DeathTable"].height,
                _objects["DeathTable"].type);
            return deathTable;
        }

        public static Enemy CreateAngryEnemy(float x, float y, Vector2 speed)
        {
            var angryEnemy = new Enemy(
                new Vector2(x, y),
                _objects["AngryEnemy"].width,
                _objects["AngryEnemy"].height,
                _objects["AngryEnemy"].type,
                ObjectTypes.angryEnemy,
                4f);
            angryEnemy.ChangeSpeed(speed.X, speed.Y);
            return angryEnemy;
        }

        public static EnemyAttack CreateAngryEnemyAttack(float x, float y, Direction direction)
        {
            var angryEnemyAttack = new EnemyAttack(
                new Vector2(x, y),
                _objects["AngryEnemyAttack"].width,
                _objects["AngryEnemyAttack"].height,
                _objects["AngryEnemyAttack"].type,
                direction,
                damage: 10);
            return angryEnemyAttack;
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
            portal,
            loseWindow,
            restartButton1,
            pauseWindow,
            pauseButton,
            exitToMenuButton,
            continueButton,
            beginGameButton,
            leaveGameButton,
            winWindow,
            restartButton2,
            clouds,
            sun,
            mountains,
            japanHouse,
            sakura,
            tree,
            deathTable,
            angryEnemy,
            angryEnemyAttack
        }
    }
}
