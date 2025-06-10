using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyGame.Model.Objects.MapObjects;
using MyGame.Model.ObjectTypes;
using NUnit.Framework.Internal.Execution;
using static MyGame.Model.Direction;

namespace MyGame.Model
{
    public static class EnemyControl
    {
        private static Dictionary<int, IAliveObject> AliveObjects;
        private static Dictionary<int, IAttackObject> AttackObjects;
        private static Dictionary<int, IMapObject> MapObjects;

        private static int PlayerId;
        public static int CurrentId { get; private set; }

        public static void ConnectEnemyControl(
            Dictionary<int, IAliveObject> aliveObjects,
            Dictionary<int, IAttackObject> attackObjects,
            Dictionary<int, IMapObject> mapObjects,
            int playerId)
        {
            AliveObjects = aliveObjects;
            AttackObjects = attackObjects;
            MapObjects = mapObjects;
            PlayerId = playerId;
        }

        public static void BeginEnemyControl(int currentId)
        {
            CurrentId = currentId;
            foreach (var aliveObject in AliveObjects.Values)
            {
                if (aliveObject is Enemy enemy)
                {
                    enemy.JumpAttempt();
                    enemy.ChangeDirection(MapObjects[PlayerId].Pos);
                    if (enemy.TryAttack())
                    {
                        IMapObject generatedObject;
                        if (enemy.Direction == right)
                            generatedObject = Factory.CreateEnemyAttack(
                                enemy.Pos.X + enemy.Width, enemy.Pos.Y - enemy.Height / 4, right);
                        else
                            generatedObject = Factory.CreateEnemyAttack(
                                enemy.Pos.X - 128, enemy.Pos.Y - enemy.Height / 2, left);
                        MapObjects.Add(CurrentId, generatedObject);
                        AttackObjects.Add(CurrentId, generatedObject as IAttackObject);
                        CurrentId++;
                    }
                }
            }
        }
    }
}
