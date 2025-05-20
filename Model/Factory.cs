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
                {"Enemy", ((byte) ObjectTypes.enemy, 64, 64) },
                {"Grass", ((byte) ObjectTypes.grass, 120, 120) },
                {"Dirt", ((byte) ObjectTypes.dirt, 120, 120) },
                {"DirtNoSolid", ((byte) ObjectTypes.dirtNoSolid, 120, 120) },
                {"PlayerAttack", ((byte) ObjectTypes.playerAttack, 16, 64) }
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

        public static PlayerAttack CreatePlayerAttack(float x, float y, IGameplayModel.Direction direction)
        {
            var playerAttack = new PlayerAttack(new Vector2(x, y),
                _objects["PlayerAttack"].width,
                _objects["PlayerAttack"].height,
                direction);
            playerAttack.ImageId = _objects["PlayerAttack"].type;
            return playerAttack;
        }

        public enum ObjectTypes : byte
        {
            player,
            enemy,
            grass,
            dirt,
            dirtNoSolid,
            playerAttack
        }
    }
}
