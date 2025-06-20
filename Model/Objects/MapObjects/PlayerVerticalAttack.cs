﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using KnightLegends.Model.ObjectTypes;
using KnightLegends.View;
using static KnightLegends.Model.Direction;

namespace KnightLegends.Model.Objects.MapObjects
{
    public class PlayerVerticalAttack : IAttackObject
    {
        public int ImageId { get; set; }
        public Vector2 Pos { get; private set; }
        public Vector2 PrevPos { get; private set; }
        public Vector2 Speed { get; private set; }
        public int Width { get; }
        public int Height { get; }

        public int AnimationTimer { get; private set; }
        public Vector2 ImagePos { get; private set; }

        public bool DestroyPermission { get; private set; }

        public RectangleCollider Collider { get; private set; }

        public Direction Direction { get; private set; }
        private int DestructionTimer { get; set; }
        public int Damage { get; private set; }


        public PlayerVerticalAttack(Vector2 position, int width, int height, int imageId, Direction direction)
        {
            ImageId = imageId;
            AnimationTimer = 0;
            ImagePos = new Vector2();
            PrevPos = Pos;
            Pos = position;
            Width = width;
            Height = height;
            DestroyPermission = false;
            Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y,
                width, height);
            Direction = direction;
            DestructionTimer = 16;
            Damage = 20;
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

        public Rectangle Animate(int widthImage)
        {
            if (AnimationTimer == 0)
            {
                ImagePos = Animation.AnimateObjectMove(Width, Height,
                    widthImage, ImagePos, Pos - PrevPos);
            }
            UpdateAnimationTimers();
            return new Rectangle((int)ImagePos.X, (int)ImagePos.Y, Width, Height);
        }

        private void CanDestroy()
        {
            if (DestructionTimer == 0)
                DestroyPermission = true;
        }

        private void UpdateAnimationTimers()
        {
            if (AnimationTimer != 0)
                AnimationTimer -= 1;
            else
                AnimationTimer = 2;
        }

        private void UpdateDestructionTimer()
        {
            DestructionTimer -= 1;
        }

        public void Update()
        {
            if (Direction == left)
                Move(-8, 0);
            else if (Direction == right)
                Move(8, 0);
            CanDestroy();
            UpdateDestructionTimer();
        }
    }
}