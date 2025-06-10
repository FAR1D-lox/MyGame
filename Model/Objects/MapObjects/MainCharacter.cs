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
using static System.Net.Mime.MediaTypeNames;
using MyGame.Model.ObjectTypes;

namespace MyGame.Model.Objects.MapObjects
{
    public class MainCharacter : IMapObject, ISolidObject, IGravityObject, IAnimationMapObject, IAliveObject
    {
        public int ImageId { get; }

        public Vector2 Pos { get; private set; }
        public Vector2 PrevPos { get; private set; }
        public int Width { get; }
        public int Height { get; }

        public RectangleCollider Collider { get; private set; }

        public Vector2 Speed { get; private set; }
        public bool IsGrounded { get; set; }
        public float JumpForce { get; private set; }
        public float Gravity { get; }
        public float VerticalSpeed { get; private set; }

        public Vector2 ImagePos { get; private set; }
        public int AnimationTimer { get; private set; }
        public int HP { get; set; }
        public int ImmortalTimer { get; private set; }

        public MainCharacter(Vector2 position, int width, int height, int imageId)
        {
            ImageId = imageId;
            AnimationTimer = 0;
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
            HP = 100;
            ImmortalTimer = 0;
        }

        public void JumpAttempt()
        {
            if (IsGrounded)
            {
                VerticalSpeed = -JumpForce;
                IsGrounded = false;
            }
        }

        public void MoveCollider()
        {
            Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y,
                Width, Height);
        }

        public void Move(float xMove, float yMove)
        {
            ChangePreviousPosition(Pos.X, Pos.Y);
            Pos += new Vector2(xMove, yMove);
            MoveCollider();
        }

        public void ChangePosition(float xPos, float yPos)
        {
            ChangePreviousPosition(Pos.X, Pos.Y);
            Pos = new Vector2(xPos, yPos);
            MoveCollider();
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
        }

        public void PushTop()
        {
            VerticalSpeed = 0;
        }

        public Rectangle Animate(int widthImage)
        {

            if (AnimationTimer <= 0)
            {
                AnimationTimer = 5;
                ImagePos = Animation.AnimateObjectMove(Width, Height,
                    widthImage, ImagePos, Pos - PrevPos);
                if (ImmortalTimer > 0)
                {
                    ImagePos = Animation.AnimateHurtObject(ImagePos, Height);
                }
            }
            AnimationTimer -= 1;
            return new Rectangle((int)ImagePos.X, (int)ImagePos.Y, Width, Height);
        }

        public void TryReduceHealthPoints(int damage)
        {
            if (ImmortalTimer <= 0)
            {
                HP -= damage;
                ImmortalTimer = 30;
            }
        }

        private void UpdateImmortalTimer()
        {
            ImmortalTimer -= 1;
        }

        public void ControlPosHeight()
        {
            if (Pos.Y > 1500)
            {
                HP = 0;
            }
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
            ControlPosHeight();
            UpdateImmortalTimer();
        }
    }
}
