using Microsoft.Xna.Framework;
using MyGame.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Model
{
    public class PlayerAttack : IObject, IAnimation
    {
        public int ImageId { get; set; }
        public Vector2 Pos { get; private set; }
        public Vector2 PrevPos { get; private set; }
        public Vector2 Speed { get; private set; }
        public int Width { get; }
        public int Height { get; }

        public int timer { get; private set; }
        public Vector2 ImagePos { get; private set; }

        public bool DestroyPermission { get; private set; }

        public RectangleCollider Collider { get; set; }

        public PlayerAttack(Vector2 position, int width, int height)
        {
            timer = 0;
            ImagePos = new Vector2();
            PrevPos = position;
            Pos = position;
            Width = width;
            Height = height;
            DestroyPermission = false;
            Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y,
                width, height);
        }

        public void Move(float xMove, float yMove)
        {
            ChangePreviousPosition(Pos.X, Pos.Y);
            Pos += new Vector2(xMove, yMove);
        }

        public void ChangePosition(float xPos, float yPos)
        {
            ChangePreviousPosition(Pos.X, Pos.Y);
            Pos = new Vector2(xPos, yPos);
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

        public Rectangle? Animate(int heightImage, int widthImage)
        {
            if (timer <= 0)
            {
                timer = 25;
                ImagePos = Animation.AnimateObject(Width, Height,
                    widthImage, heightImage, ImagePos, Pos - PrevPos);
            }
            timer -= 1;
            if (timer != 0)
            {
                return new Rectangle((int)ImagePos.X, (int)ImagePos.Y, Width, Height);
            }
            else
            {
                MayDestroy();
                return null;
            }
        }

        private void MayDestroy()
        {
            DestroyPermission = true;
        }

        public void Update()
        {
            
        }
    }
}
