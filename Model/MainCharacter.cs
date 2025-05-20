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
        public Vector2 PrevPos { get; set; }
        private Vector2 _speed;
        public int Width { get; }
        public int Height { get; }

        public RectangleCollider Collider { get; set; }

        public Vector2 Speed
        {
            get { return _speed; }
            set
            {
                _speed = value;
                if (_speed.X > 5)
                    _speed.X = 5;
                else if (_speed.X < -5)
                    _speed.X = -5;
            }
        }

        public bool isGrounded { get; set; }
        public float jumpForce { get; set; }
        public float gravity { get; set; }
        public float verticalSpeed { get; set; }

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
            jumpForce = 15f;
            gravity = 0.5f;
            verticalSpeed = 0f;
            isGrounded = false;
        }

        public void JumpAttempt()
        {
            if (isGrounded)
            {
                verticalSpeed = -jumpForce;
                isGrounded = false;
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

        public Rectangle Animate(int widthImage, int heightImage)
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
            Move(Speed);
            if (Speed.X != 0)
            {
                if (Speed.X < -1e-3)
                    Speed = new Vector2(Speed.X + 0.2f, Speed.Y);
                if (Speed.X > 1e-3)
                    Speed = new Vector2(Speed.X - 0.2f, Speed.Y);
            }
        }

    }
}
