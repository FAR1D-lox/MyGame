using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyGame.Model
{
    public static class Factory
    {
        private static readonly Dictionary<string, (byte type, int width, int height)> _objects = new()
            {
                {"MainCharacter", ((byte) ObjectTypes.player, 34, 59) },
                {"Enemy", ((byte) ObjectTypes.enemy, 37, 64) },
                {"Grass", ((byte) ObjectTypes.grass, 120, 120) },
                {"Dirt", ((byte) ObjectTypes.dirt, 120, 120) },
                {"DirtNoSolid", ((byte) ObjectTypes.dirtNoSolid, 120, 120) },
                {"PlayerVerticalAttack", ((byte) ObjectTypes.playerVerticalAttack, 16, 64) },
                {"PlayerHorisontalAttack", ((byte) ObjectTypes.playerHorisontalAttack, 64, 16) },
                {"EnemyAttack", ((byte) ObjectTypes.enemyAttack, 128, 128) },
                {"LoseWindow", ((byte) ObjectTypes.loseWindow, 960, 540) },
                {"RestartButton", ((byte) ObjectTypes.restartButton, 480, 150) }
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

        public static BlockDirt CreateDirt(float x, float y)
        {
            var dirt = new BlockDirt(
                new Vector2(x, y),
                _objects["Dirt"].width,
                _objects["Dirt"].height,
                _objects["Dirt"].type);
            return dirt;
        }

        public static BlockDirtNoSolid CreateDirtNoSolid(float x, float y)
        {
            var dirtNoSolid = new BlockDirtNoSolid(
                new Vector2(x, y),
                _objects["DirtNoSolid"].width,
                _objects["DirtNoSolid"].height,
                _objects["DirtNoSolid"].type);
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

        public enum ObjectTypes : byte
        {
            player,
            enemy,
            grass,
            dirt,
            dirtNoSolid,
            playerVerticalAttack,
            playerHorisontalAttack,
            enemyAttack,
            loseWindow,
            restartButton
        }
    }
}
