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

namespace MyGame.Model
{
    public class MainCharacter : IObject, ISolid, IGravity, IAnimation
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

        public Vector2 ImagePos { get; private set; }
        public int timer { get; private set; }

        public MainCharacter(Vector2 position, int width, int height)
        {
            timer = 0;
            ImagePos = new Vector2();
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
        }

        public void JumpAttempt()
        {
            if (IsGrounded)
            {
                VerticalSpeed = -JumpForce;
                IsGrounded = false;
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
            if (Speed.X > 5)
                ChangeSpeed(5, ySpeed);
            if (Speed.X < -5)
                ChangeSpeed(-5, ySpeed);
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

        public Rectangle? Animate(int widthImage, int heightImage)
        {
            if (timer <= 0)
            {
                timer = 5;
                ImagePos = Animation.AnimateObject(Width, Height,
                    widthImage, heightImage, ImagePos, Pos - PrevPos);
            }
            timer -= 1;
            return new Rectangle((int)ImagePos.X, (int)ImagePos.Y, Width, Height);
        }


        public void Update()
        {
            Move(Speed.X, Speed.Y);
            if (Speed.X != 0)
            {
                if (Speed.X < -1e-3)
                    ChangeSpeed(Speed.X + 0.2f, Speed.Y);
                if (Speed.X > 1e-3)
                    ChangeSpeed(Speed.X - 0.2f, Speed.Y);
            }
        }
    }
}
