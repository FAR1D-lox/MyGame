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
using static MyGame.Model.MouseClick;



namespace MyGame.Model
{
    public static class PlayerControl
    {
        private static Dictionary<int, IObject> Objects;
        private static Dictionary<int, IAttackObject> AttackObjects;
        private static int PlayerId;
        private static int CurrentId;
        private static Direction Direction;
        private static Direction PrevDirection;

        public static void ConnectPlayerControl(Dictionary<int, IObject> objects,
            Dictionary<int, IAttackObject> attackObjects)
        {
            Objects = objects;
            AttackObjects = attackObjects;
        }

        public static List<int> BeginPlayerControl(int playerId, int currentId, ControlsEventArgs e)
        {
            PlayerId = playerId;
            CurrentId = currentId;
            TryAttackAndChangeSpeed(e);
            return new List<int> { PlayerId, CurrentId };
        }

        private static void TryAttackAndChangeSpeed(ControlsEventArgs e)
        {
            if (Direction != None && Direction != up)
            PrevDirection = Direction;
            Direction = e.Direction;
            if (e.MouseLeftButtonState == pressed)
            {
                PlayerAttack();
            }
            ChangePlayerSpeed(e.Direction);
        }

        private static void PlayerAttack()
        {
            MainCharacter player = Objects[PlayerId] as MainCharacter;
            IObject generatedObject = null;

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

            Objects.Add(CurrentId, generatedObject);
            AttackObjects.Add(CurrentId, generatedObject as IAttackObject);
            CurrentId++;
        }

        private static void ChangePlayerSpeed(Direction direction)
        {
            MainCharacter player = Objects[PlayerId] as MainCharacter;
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
