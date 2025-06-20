﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using KnightLegends.Model.Objects.MapObjects;
using KnightLegends.Model.ObjectTypes;

namespace KnightLegends.Model
{
    public static class AttacksControl
    {
        private static Dictionary<int, IAliveObject> AliveObjects;
        private static Dictionary<int, IGravityObject> GravityObjects;
        private static Dictionary<int, ISolidObject> SolidObjects;
        private static Dictionary<int, IAttackObject> AttackObjects;
        private static Dictionary<int, IMapObject> MapObjects;

        public static void ConnectAttacksControl(
            Dictionary<int, IAliveObject> aliveObjects,
            Dictionary<int, IGravityObject> gravityObjects,
            Dictionary<int, ISolidObject> solidObjects,
            Dictionary<int, IAttackObject> attackObjects,
            Dictionary<int, IMapObject> mapObjects)
        {
            AliveObjects = aliveObjects;
            GravityObjects = gravityObjects;
            SolidObjects = solidObjects;
            AttackObjects = attackObjects;
            MapObjects = mapObjects;
        }

        public static void ActivateAttacksControl()
        {
            TryHurtAliveObjects();
            TryDestroyAliveObjects();
            TryDestroyAttacks();
        }
        private static void TryHurtAliveObjects()
        {
            foreach (var attackObject in AttackObjects.Values)
            {
                foreach (var aliveObject in AliveObjects.Values)
                {
                    if (!((aliveObject is MainCharacter &&
                        (attackObject is PlayerHorisontalAttack ||
                        attackObject is PlayerVerticalAttack)) ||
                        (aliveObject is Enemy &&
                        attackObject is EnemyAttack)))
                    {
                        if (RectangleCollider.IsCollided(aliveObject.Collider, attackObject.Collider))
                        {
                            aliveObject.TryReduceHealthPoints(attackObject.Damage);
                        }
                    }
                }
            }
        }

        private static void TryDestroyAliveObjects()
        {
            foreach (var aliveObjectId in AliveObjects.Keys)
            {
                if (AliveObjects[aliveObjectId].HP <= 0)
                {
                    MapObjects.Remove(aliveObjectId);
                    AliveObjects.Remove(aliveObjectId);
                    if (GravityObjects.ContainsKey(aliveObjectId))
                        GravityObjects.Remove(aliveObjectId);
                    if (SolidObjects.ContainsKey(aliveObjectId))
                        SolidObjects.Remove(aliveObjectId);
                }
            }
        }

        private static void TryDestroyAttacks()
        {
            foreach (var attackId in AttackObjects.Keys)
            {
                if (AttackObjects[attackId].DestroyPermission)
                {
                    MapObjects.Remove(attackId);
                    AttackObjects.Remove(attackId);
                }
            }
        }
    }
}
