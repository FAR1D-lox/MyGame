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
    public class BlockDirtNoSolid : IMapObject
    {
        public int ImageId { get; }
        public Vector2 Pos { get; set; }
        public Vector2 PrevPos { get; set; }
        public Vector2 Speed { get; private set; }
        public int Width { get; }
        public int Height { get; }

        public BlockDirtNoSolid(Vector2 position, int width, int height, int imageId)
        {
            ImageId = imageId;
            PrevPos = position;
            Pos = position;
            Width = width;
            Height = height;
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
        }

        public void Update()
        {

        }
    }
}