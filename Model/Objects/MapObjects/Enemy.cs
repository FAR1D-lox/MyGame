using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using KnightLegends.Model.ObjectTypes;
using KnightLegends.View;
using static KnightLegends.Model.Direction;

namespace KnightLegends.Model.Objects.MapObjects
{
    public class Enemy : IMapObject, ISolidObject, IGravityObject, IAnimationMapObject, IAliveObject
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
        public int timer { get; set; }
        public Vector2 ImagePos { get; private set; }
        public int AnimationTimer { get; private set; }
        public Direction Direction { get; private set; }
        public int HP { get; set; }
        public int ImmortalTimer { get; private set; }
        public int AttackTimer { get; private set; }
        public float BaseEnemySpeed { get; }

        public Factory.ObjectTypes Type { get; }

        private Direction AttackDirection;

        public Enemy(Vector2 position, int width, int height, int imageId, Factory.ObjectTypes type, float baseEnemySpeed)
        {
            ImageId = imageId;
            AnimationTimer = 0;
            ImagePos = new Vector2();
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
            Direction = None;
            HP = 100;
            ImmortalTimer = 0;
            AttackTimer = 60;
            AttackDirection = None;
            Type = type;
            BaseEnemySpeed = baseEnemySpeed;
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
        }

        private void ChangeEnemySpeed()
        {
            if (Direction == left)
            {
                if (!CollisionCalculater.CheckLeftSide(this))
                    ChangeSpeed(-BaseEnemySpeed, Speed.Y);
                else
                    ChangeSpeed(0, Speed.Y);
            }
            else if (Direction == right)
            {
                if (!CollisionCalculater.CheckRightSide(this))
                    ChangeSpeed(BaseEnemySpeed, Speed.Y);
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
        }

        public void PushTop()
        {
            VerticalSpeed = 0;
        }

        public void Update()
        {
            if (AttackTimer <= 60 && AttackTimer > 40)
            {
                ChangeSpeed(0, Speed.Y);
            }
            else
            {
                ChangeEnemySpeed();
            }
            Move(Speed.X, Speed.Y);
            MoveCollider();
            UpdateAttackTimer();
            UpdateImmortalTimer();
            ControlPosHeight();
        }


        public void ChangeDirection(Vector2 playerPos)
        {
            var differentCoordinates = playerPos - Pos;
            if (differentCoordinates.X > -1e-1 && differentCoordinates.X < 700)
                Direction = right;
            else if (differentCoordinates.X < 1e-1 && differentCoordinates.X > -700)
                Direction = left;
            else
                Direction = None;
        }

        public bool TryAttack()
        {
            if (AttackTimer <= 0)
            {
                if (Pos.X - PrevPos.X < 0)
                    AttackDirection = left;
                else if (Pos.X - PrevPos.X > 0)
                    AttackDirection = right;
                AttackTimer = 60;
                return true;
            }
            return false;
        }

        private void UpdateAttackTimer()
        {
            AttackTimer -= 1;
        }

        public void TryReduceHealthPoints(int damage)
        {
            if (ImmortalTimer <= 0)
            {
                HP -= damage;
                ImmortalTimer = 30;
            }

        }

        public void ControlPosHeight()
        {
            if (Pos.Y > 2050)
            {
                HP = 0;
            }
        }
        private void UpdateImmortalTimer()
        {
            ImmortalTimer -= 1;
        }

        public Rectangle Animate(int widthImage)
        {
            if (AnimationTimer <= 0)
            {
                AnimationTimer = 5;
                if (AttackTimer >= 40 && AttackTimer <= 60)
                {
                    ImagePos = Animation.AnimateObjectAttacking(Width, Height,
                        widthImage, ImagePos, AttackDirection);
                }
                else
                {
                    ImagePos = Animation.AnimateObjectMove(Width, Height,
                        widthImage, ImagePos, Pos - PrevPos);
                }
                if (ImmortalTimer > 0)
                {
                    ImagePos = Animation.AnimateHurtObject(ImagePos, Height);
                }
            }
            AnimationTimer -= 1;
            return new Rectangle((int)ImagePos.X, (int)ImagePos.Y, Width, Height);
        }
    }
}