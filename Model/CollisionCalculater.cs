using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyGame.Model
{
    public static class CollisionCalculater
    {
        static Dictionary<int, IObject> Objects = new();
        static Dictionary<int, ISolid> SolidObjects = new();
        static Dictionary<int, IGravity> GravityObjects = new();

        public static void ActivateCollisionCalculater(Dictionary<int, IObject> objects,
            Dictionary<int, ISolid> solidObjects, Dictionary<int, IGravity> gravityObjects)
        {
            Objects = objects;
            SolidObjects = solidObjects;
            GravityObjects = gravityObjects;
            CalculateCollisionPreparation();
            CheckGroundedObjects();
        }

        private static void CalculateCollisionPreparation()
        {
            var collisionObjects = new Dictionary<int, Vector2>();
            FillCollisionObjects(collisionObjects);
            CycleCaclulatesObstacleCollision(collisionObjects);
        }

        private static void FillCollisionObjects(Dictionary<int, Vector2> collisionObjects)
        {
            foreach (var Id in Objects.Keys)
            {
                Vector2 initPos = Objects[Id].Pos;
                Objects[Id].ChangePreviousPosition(initPos.X, initPos.Y);
                Objects[Id].Update();
                if (SolidObjects.ContainsKey(Id))
                    collisionObjects.Add(Id, initPos);
            }
        }

        private static void CycleCaclulatesObstacleCollision(Dictionary<int, Vector2> collisionObjects)
        {
            var processedObjects = new HashSet<(int, int)>();
            foreach (var Id1 in collisionObjects.Keys)
            {
                foreach (var Id2 in collisionObjects.Keys)
                {
                    if (Id1 == Id2 || processedObjects.Contains((Id2, Id1)))
                        continue;
                    var changes = CalculateObstacleCollision(
                        (collisionObjects[Id1], Id1),
                        (collisionObjects[Id2], Id2));
                    collisionObjects[Id1] = changes.First();
                    collisionObjects[Id2] = changes.Last();
                    processedObjects.Add((Id1, Id2));
                }
            }
        }
        
        private static HashSet<Vector2> CalculateObstacleCollision
        (
            (Vector2 initPos, int Id) obj1,
            (Vector2 initPos, int Id) obj2
        )
        {
            var isCollided = CalculateReverseObjectMove(obj1, obj2);

            if (isCollided)
            {
                if (GravityObjects.ContainsKey(obj1.Id))
                    CalculateGravityMove(obj1, obj2);
                if (GravityObjects.ContainsKey(obj2.Id))
                    CalculateGravityMove(obj2, obj1);
            }
            return new HashSet<Vector2> { obj1.initPos, obj2.initPos };
        }

        private static void CalculateGravityMove
        (
            (Vector2 initPos, int Id) obj1,
            (Vector2 initPos, int Id) obj2
        )
        {
            var gravityObj1 = GravityObjects[obj1.Id];
            if (CheckIfGrounded(gravityObj1) || CheckTop(gravityObj1))
            {
                if (CheckLeftSide(gravityObj1) || CheckRightSide(gravityObj1))
                {
                    gravityObj1.ChangeSpeed(0, 0);
                }
                else
                {
                    gravityObj1.ChangeSpeed(gravityObj1.Speed.X, 0);
                }
                gravityObj1.PushTop();
                CalculateReverseObjectMove(obj1, obj2);
            }

            if (!CheckIfGrounded(gravityObj1))
            {
                obj1.initPos = gravityObj1.Pos;
                if (CheckLeftSide(gravityObj1) || CheckRightSide(gravityObj1))
                {
                    gravityObj1.ChangeSpeed(0, gravityObj1.Speed.Y + gravityObj1.VerticalSpeed);
                }
                else
                {
                    gravityObj1.ChangeSpeed(gravityObj1.Speed.X, gravityObj1.Speed.Y + gravityObj1.VerticalSpeed);
                }
                gravityObj1.Move(0, gravityObj1.VerticalSpeed);
                CalculateReverseObjectMove(obj1, obj2);
            }
        }

        private static bool CalculateReverseObjectMove(
            (Vector2 initPos, int Id) obj1,
            (Vector2 initPos, int Id) obj2)
        {
            var oppositeDirection = new Vector2();
            var isCollided = false;
            var cycleCount = 0;
            while (RectangleCollider.IsCollided(
                SolidObjects[obj1.Id].Collider,
                SolidObjects[obj2.Id].Collider))
            {
                isCollided = true;
                TryReverseMove(obj1, oppositeDirection);
                TryReverseMove(obj2, oppositeDirection);
                if (cycleCount == 150)
                {
                    Console.WriteLine("Ошибка, связанная с коллизиями!!!");
                    break;
                }
                cycleCount++;
            }
            return isCollided;
        }

        private static void TryReverseMove((Vector2 initPos, int Id) obj,
            Vector2 oppositeDirection)
        {
            if (obj.initPos != Objects[obj.Id].Pos)
            {
                oppositeDirection = Objects[obj.Id].Pos - obj.initPos;
                oppositeDirection.Normalize();
                Objects[obj.Id].Move(-oppositeDirection.X, -oppositeDirection.Y);
            }
        }
        public static bool CheckIfGrounded(IGravity obj)
        {
            var solidObj = obj as ISolid;
            var feetCheck = new RectangleCollider(
                (int)obj.Pos.X,
                (int)obj.Pos.Y + obj.Height + 2,
                obj.Width,
                1);

            foreach (var otherSolidObj in SolidObjects.Values)
            {
                if (otherSolidObj != solidObj && RectangleCollider.IsCollided(feetCheck, otherSolidObj.Collider))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool CheckTop(IGravity obj)
        {
            var solidObj = obj as ISolid;
            var upCheck = new RectangleCollider(
                (int)obj.Pos.X,
                (int)obj.Pos.Y - 2,
                obj.Width,
                1);

            foreach (var otherSolidObj in SolidObjects.Values)
            {
                if (otherSolidObj != solidObj && RectangleCollider.IsCollided(upCheck, otherSolidObj.Collider))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool CheckLeftSide(IGravity obj)
        {
            var solidObj = obj as ISolid;
            var leftCheck = new RectangleCollider(
                (int)obj.Pos.X - 2,
                (int)obj.Pos.Y,
                1,
                obj.Height);

            foreach (var otherSolidObj in SolidObjects.Values)
            {
                if (otherSolidObj != solidObj && RectangleCollider.IsCollided(leftCheck, otherSolidObj.Collider))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool CheckRightSide(IGravity obj)
        {
            var solidObj = obj as ISolid;
            var rightCheck = new RectangleCollider(
                (int)obj.Pos.X + obj.Width + 2,
                (int)obj.Pos.Y,
                1,
                obj.Height);

            foreach (var otherSolidObj in SolidObjects.Values)
            {
                if (otherSolidObj != solidObj && RectangleCollider.IsCollided(rightCheck, otherSolidObj.Collider))
                {
                    return true;
                }
            }
            return false;
        }

        private static void CheckGroundedObjects()
        {
            foreach (var gravityObject in GravityObjects.Values)
            {
                gravityObject.IsGrounded = CheckIfGrounded(gravityObject);
            }
        }
    }
}