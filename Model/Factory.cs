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
        private static Dictionary<string, (byte type, int width, int height)> _objects =
            new Dictionary<string, (byte type, int width, int height)>()
            {
                {"MainCharacter", ((byte) ObjectTypes.player, 34, 59) },
                {"Enemy", ((byte) ObjectTypes.enemy, 37, 64) },
                {"Grass", ((byte) ObjectTypes.grass, 120, 120) },
                {"Dirt", ((byte) ObjectTypes.dirt, 120, 120) },
                {"DirtNoSolid", ((byte) ObjectTypes.dirtNoSolid, 120, 120) },
                {"PlayerVerticalAttack", ((byte) ObjectTypes.playerVerticalAttack, 16, 64) },
                {"PlayerHorisontalAttack", ((byte) ObjectTypes.playerHorisontalAttack, 64, 16) },
                {"EnemyAttack", ((byte) ObjectTypes.enemyAttack, 128, 128) },
            };
        public static MainCharacter CreateMainCharacter(float x, float y, Vector2 speed)
        {
            var player = new MainCharacter(
                new Vector2(x, y),
                _objects["MainCharacter"].width,
                _objects["MainCharacter"].height);
            player.ImageId = _objects["MainCharacter"].type;
            player.ChangeSpeed(speed.X, speed.Y);
            return player;
        }

        public static Enemy CreateEnemy(float x, float y, Vector2 speed)
        {
            var enemy = new Enemy(
                new Vector2(x, y),
                _objects["Enemy"].width,
                _objects["Enemy"].height);
            enemy.ImageId = _objects["Enemy"].type;
            enemy.ChangeSpeed(speed.X, speed.Y);
            return enemy;
        }

        public static BlockGrass CreateGrass(float x, float y)
        {
            var grass = new BlockGrass(
                new Vector2(x, y),
                _objects["Grass"].width,
                _objects["Grass"].height);
            grass.ImageId = _objects["Grass"].type;
            return grass;
        }

        public static BlockDirt CreateDirt(float x, float y)
        {
            var dirt = new BlockDirt(
                new Vector2(x, y),
                _objects["Dirt"].width,
                _objects["Dirt"].height);
            dirt.ImageId = _objects["Dirt"].type;
            return dirt;
        }

        public static BlockDirtNoSolid CreateDirtNoSolid(float x, float y)
        {
            var dirtNoSolid = new BlockDirtNoSolid(new Vector2(x, y),
                _objects["DirtNoSolid"].width,
                _objects["DirtNoSolid"].height);
            dirtNoSolid.ImageId = _objects["DirtNoSolid"].type;
            return dirtNoSolid;
        }

        public static PlayerVerticalAttack CreatePlayerVecticalAttack(float x, float y, Direction direction)
        {
            var playerVerticalAttack = new PlayerVerticalAttack(new Vector2(x, y),
                _objects["PlayerVerticalAttack"].width,
                _objects["PlayerVerticalAttack"].height,
                direction);
            playerVerticalAttack.ImageId = _objects["PlayerVerticalAttack"].type;
            return playerVerticalAttack;
        }

        public static PlayerHorisontalAttack CreatePlayerHorisontalAttack(float x, float y, Direction direction)
        {
            var playerHorisontalAttack = new PlayerHorisontalAttack(new Vector2(x, y),
                _objects["PlayerHorisontalAttack"].width,
                _objects["PlayerHorisontalAttack"].height,
                direction);
            playerHorisontalAttack.ImageId = _objects["PlayerHorisontalAttack"].type;
            return playerHorisontalAttack;
        }

        public static EnemyAttack CreateEnemyAttack(float x, float y, Direction direction)
        {
            var enemyAttack = new EnemyAttack(new Vector2(x, y),
                _objects["EnemyAttack"].width,
                _objects["EnemyAttack"].height,
                direction);
            enemyAttack.ImageId = _objects["EnemyAttack"].type;
            return enemyAttack;
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
            enemyAttack
        }
    }
}
