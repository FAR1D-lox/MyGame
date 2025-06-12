using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using MyGame.View;
using MonoGame.Framework.Utilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using static MyGame.Model.Direction;
using static Microsoft.Xna.Framework.Input.ButtonState;
using MyGame.Presenter;
using MyGame.Model.ObjectTypes;
using MyGame.Model.Objects.MapObjects;

namespace MyGame.Model
{
    public static class PlayerControl
    {
        private static Dictionary<int, IMapObject> MapObjects;
        private static Dictionary<int, IAttackObject> AttackObjects;
        public static int PlayerId { get; set; }
        public static int CurrentId { get; set; }
        private static Direction Direction;
        private static Direction PrevDirection;
        private static int attacksTimer = 0;

        public static void ConnectPlayerControl(
            Dictionary<int, IMapObject> mapObjects,
            Dictionary<int, IAttackObject> attackObjects)
        {
            MapObjects = mapObjects;
            AttackObjects = attackObjects;
        }

        public static void BeginPlayerControl(int playerId, int currentId, MainCharacterControlData e)
        {
            PlayerId = playerId;
            CurrentId = currentId;
            TryAttackAndChangeSpeed(e);
        }

        private static void TryAttackAndChangeSpeed(MainCharacterControlData e)
        {
            if (Direction != None && Direction != up)
                PrevDirection = Direction;
            Direction = e.Direction;
            if (e.MouseLeftButtonState == Pressed)
            {
                PlayerAttack();
            }
            ChangePlayerSpeed(e.Direction);
        }

        private static void PlayerAttack()
        {
            MainCharacter player = MapObjects[PlayerId] as MainCharacter;
            IMapObject generatedObject = null;

            if (attacksTimer <= 0)
            {
                if (Direction == right
                    || Direction == rightUp)
                {
                    generatedObject = Factory.CreatePlayerHorisontalAttack(
                        player.Pos.X + player.Width, player.Pos.Y + player.Height / 2, right);
                }
                else if (Direction == left
                    || Direction == leftUp)
                {
                    generatedObject = Factory.CreatePlayerHorisontalAttack(
                        player.Pos.X - 64, player.Pos.Y + player.Height / 2, left);
                }
                else if (PrevDirection == right
                    || PrevDirection == rightUp)
                {
                    generatedObject = Factory.CreatePlayerVecticalAttack(
                        player.Pos.X + player.Width, player.Pos.Y, right);
                }
                else if (PrevDirection == left
                    | PrevDirection == leftUp)
                {
                    generatedObject = Factory.CreatePlayerVecticalAttack(
                        player.Pos.X - 16, player.Pos.Y, left);
                }

                MapObjects.Add(CurrentId, generatedObject);
                AttackObjects.Add(CurrentId, generatedObject as IAttackObject);
                CurrentId++;
                attacksTimer = 15;
            }
            attacksTimer -= 1;
        }

        private static void ChangePlayerSpeed(Direction direction)
        {
            MainCharacter player = MapObjects[PlayerId] as MainCharacter;
            switch (direction)
            {
                case right:
                    {
                        if (!CollisionCalculater.CheckRightSide(player))
                            player.SpeedUp(1, 0);
                        break;
                    }
                case left:
                    {
                        if (!CollisionCalculater.CheckLeftSide(player))
                            player.SpeedUp(-1, 0);
                        break;
                    }
                case up:
                    {
                        if (!CollisionCalculater.CheckTop(player))
                            player.JumpAttempt();
                        break;
                    }
                case leftUp:
                    {
                        if (!CollisionCalculater.CheckLeftSide(player))
                            player.SpeedUp(-1, 0);
                        if (!CollisionCalculater.CheckTop(player))
                            player.JumpAttempt();
                        break;
                    }
                case rightUp:
                    {
                        if (!CollisionCalculater.CheckRightSide(player))
                            player.SpeedUp(1, 0);
                        if (!CollisionCalculater.CheckTop(player))
                            player.JumpAttempt();
                        break;
                    }
            }
        }
    }
}
