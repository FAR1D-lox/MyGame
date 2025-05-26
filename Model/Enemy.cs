using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyGame.View;

namespace MyGame.Model
{
    public class Enemy : IObject, ISolid, IGravity
    {
        public int ImageId { get; set; }

        public Vector2 Pos { get; private set; }
        public Vector2 PrevPos { get; private set; }
        public int Width { get; }
        public int Height { get; }

        public RectangleCollider Collider { get; private set; }

        public Vector2 Speed { get; private set; }

        //public bool IsGrounded { get; private set; }
        public bool IsGrounded { get; set; }
        public float JumpForce { get; private set; }
        public float Gravity { get; }
        public float VerticalSpeed { get; private set; }
        public int timer { get; set; }
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
            JumpForce = 15f;
            Gravity = 0.5f;
            VerticalSpeed = 0f;
            IsGrounded = false;
            Direction = IGameplayModel.Direction.None;
        }

        public void JumpAttempt()
        {
            if (IsGrounded && !CollisionCalculater.CheckTop(this))
            {
                if (timer <= 0)
                {
                    VerticalSpeed = -JumpForce;
                    IsGrounded = false;
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

        public void Move(float xMove, float yMove)
        {
            ChangePreviousPosition(Pos.X, Pos.Y);
            Pos += new Vector2(xMove, yMove);
            MoveCollider(Pos);
        }

        public void ChangePosition(float xPos, float yPos)
        {
            ChangePreviousPosition(Pos.X, Pos.Y);
            Pos = new Vector2(xPos, yPos);
            MoveCollider(Pos);
        }

        public void ChangePreviousPosition(float xPos, float yPos)
        {
            PrevPos = new Vector2(xPos, yPos);
        }

        public void ChangeSpeed(float xSpeed, float ySpeed)
        {
            Speed = new Vector2(xSpeed, ySpeed);
        }

        public void SpeedUp(float xSpeed, float ySpeed)
        {
            Speed += new Vector2(xSpeed, ySpeed);
        }

        private void ChangeSpeed()
        {
            if (Direction == IGameplayModel.Direction.left)
            {
                if (!CollisionCalculater.CheckLeftSide(this))
                    ChangeSpeed(-2f, Speed.Y);
                else
                    ChangeSpeed(0, Speed.Y);
            }
            else if (Direction == IGameplayModel.Direction.right)
            {
                if (!CollisionCalculater.CheckRightSide(this))
                    ChangeSpeed(2f, Speed.Y);
                else
                    ChangeSpeed(0, Speed.Y);
            }
            else
                ChangeSpeed(0, Speed.Y);
        }

        public void UpdateGravity()
        {
            if (!IsGrounded)
            {
                VerticalSpeed += Gravity;
                ChangeSpeed(Speed.X, VerticalSpeed);
            }
            else
            {
                VerticalSpeed = 0;
            }
            //IsGrounded = CollisionCalculater.CheckIfGrounded(this);
        }

        public void PushTop()
        {
            VerticalSpeed = 0;
        }

        public void Update()
        {
            ChangeSpeed();
            Pos += Speed;
            MoveCollider(Pos);
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