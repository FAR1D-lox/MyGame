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
    public class Enemy : IObject, ISolid, IGravity
    {
        public int ImageId { get; set; }

        public Vector2 Pos { get; set; }
        public Vector2 PrevPos { get; set; }
        public Vector2 Speed { get; set; }
        public int Width { get; }
        public int Height { get; }
        public RectangleCollider Collider { get; set; }
        public bool isGrounded { get; set; }
        public float jumpForce { get; set; }
        public float gravity { get; set; }
        public float verticalSpeed { get; set; }
        public int timer {  get; set; }
        private IGameplayModel.Direction Direction { get; set; }

        public Enemy(Vector2 position, int width, int height)
        {
            timer = 0;
            PrevPos = position;
            Pos = position;
            Width = width;
            Height = height;
            Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y,
                width, height);
            jumpForce = 15f;
            gravity = 0.5f;
            verticalSpeed = 0f;
            isGrounded = false;
            Direction = IGameplayModel.Direction.left;
        }

        public void JumpAttempt()
        {
            if (isGrounded && !CollisionCalculater.CheckTop(this))
            {
                if (timer <= 0)
                {
                    verticalSpeed = -jumpForce;
                    isGrounded = false;
                    timer = 50;
                }
                timer -= 1;
            }
        }

        public void MoveCollider(Vector2 newPosition)
        {
            Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y,
                Width, Height);
        }

        public void Move(Vector2 positionChange)
        {
            PrevPos = new Vector2(Pos.X, Pos.Y);
            Pos += positionChange;
            MoveCollider(Pos);
        }

        public void ChangePosition(Vector2 newPosition)
        {
            PrevPos = new Vector2(Pos.X, Pos.Y);
            Pos = newPosition;
            MoveCollider(Pos);
        }

        public void Update()
        {
            ChangeSpeed();
            Pos += Speed;
            MoveCollider(Pos);
        }

        private void ChangeSpeed()
        {
            if (Direction == IGameplayModel.Direction.left)
            {
                if (!CollisionCalculater.CheckLeftSide(this))
                    Speed = new Vector2(-2f, Speed.Y);
                else
                    Speed = new Vector2(0, Speed.Y);
            }
            else if (Direction == IGameplayModel.Direction.right)
            {
                if (!CollisionCalculater.CheckRightSide(this))
                    Speed = new Vector2(2f, Speed.Y);
                else
                    Speed = new Vector2(0, Speed.Y);
            }
            else
                Speed = new Vector2(0, Speed.Y);
        }

        public void ChangeDirection(Vector2 playerPos)
        {
            var differentCoordinates = playerPos - Pos;
            if (differentCoordinates.X > 1e-1)
                Direction = IGameplayModel.Direction.right;
            else if (differentCoordinates.X < 1e-1)
                Direction = IGameplayModel.Direction.left;
            else
                Direction = IGameplayModel.Direction.None;
        }
    }
}
