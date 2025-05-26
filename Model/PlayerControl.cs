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


namespace MyGame.Model
{
    public static class PlayerControl
    {
        private static Dictionary<int, IObject> Objects = new();
        private static int PlayerId;
        private static int CurrentId;
        private static int AttackId;
        private static IGameplayModel.Direction Direction;
        private static IGameplayModel.Direction PrevDirection;

        public static List<int> BeginPlayerControl(Dictionary<int, IObject> objects, int playerId, int currentId, int attackId, ControlsEventArgs e)
        {
            Objects = objects;
            PlayerId = playerId;
            CurrentId = currentId;
            AttackId = attackId;
            aboba(e);
            return new List<int> { PlayerId, CurrentId, AttackId };
        }

        private static void aboba(ControlsEventArgs e)
        {
            if (Direction != IGameplayModel.Direction.None && Direction != IGameplayModel.Direction.up)
            PrevDirection = Direction;
            Direction = e.direction;
            if (e.MouseLeftBottomState == IGameplayModel.MouseClick.pressed)
            {
                PlayerAttack();
            }
            ChangePlayerSpeed(e.direction);
        }

        private static void PlayerAttack()
        {
            MainCharacter player = Objects[PlayerId] as MainCharacter;

            if (Direction == IGameplayModel.Direction.right || Direction == IGameplayModel.Direction.rightUp)
            {
                Objects.Add(CurrentId, Factory.CreatePlayerHorisontalAttack(
                    player.Pos.X + player.Width, player.Pos.Y + player.Height/2, IGameplayModel.Direction.right));
            }
            else if (Direction == IGameplayModel.Direction.left || Direction == IGameplayModel.Direction.leftUp)
            {
                Objects.Add(CurrentId, Factory.CreatePlayerHorisontalAttack(
                    player.Pos.X, player.Pos.Y + player.Height / 2, IGameplayModel.Direction.left));
            }
            else if (PrevDirection == IGameplayModel.Direction.right || PrevDirection == IGameplayModel.Direction.rightUp)
            {
                Objects.Add(CurrentId, Factory.CreatePlayerVecticalAttack(
                    player.Pos.X + player.Width, player.Pos.Y, IGameplayModel.Direction.right));
            }
            else if (PrevDirection == IGameplayModel.Direction.left || PrevDirection == IGameplayModel.Direction.leftUp)
            {
                Objects.Add(CurrentId, Factory.CreatePlayerVecticalAttack(
                    player.Pos.X, player.Pos.Y, IGameplayModel.Direction.left));
            }

            AttackId = CurrentId;
            CurrentId++;
        }

        private static void ChangePlayerSpeed(IGameplayModel.Direction direction)
        {
            MainCharacter player = Objects[PlayerId] as MainCharacter;
            switch (direction)
            {
                case IGameplayModel.Direction.right:
                    {
                        if (!CollisionCalculater.CheckRightSide(player))
                            player.SpeedUp(1, 0);
                        break;
                    }
                case IGameplayModel.Direction.left:
                    {
                        if (!CollisionCalculater.CheckLeftSide(player))
                            player.SpeedUp(-1, 0);
                        break;
                    }
                case IGameplayModel.Direction.up:
                    {
                        if (!CollisionCalculater.CheckTop(player))
                            player.JumpAttempt();
                        break;
                    }
                case IGameplayModel.Direction.leftUp:
                    {
                        if (!CollisionCalculater.CheckLeftSide(player))
                            player.SpeedUp(-1, 0);
                        if (!CollisionCalculater.CheckTop(player))
                            player.JumpAttempt();
                        break;
                    }
                case IGameplayModel.Direction.rightUp:
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
