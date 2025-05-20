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
    public class BlockDirt : IObject, ISolid
    {
        public int ImageId { get; set; }
        public Vector2 Pos { get; set; }
        public Vector2 PrevPos { get; set; }
        public Vector2 Speed { get; set; }
        public int Width { get; }
        public int Height { get; }
        public RectangleCollider Collider { get; set; }

        public BlockDirt(Vector2 position, int width, int height)
        {
            PrevPos = position;
            Pos = position;
            Width = width;
            Height = height;
            Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y,
                width, height);
        }

        public void MoveCollider(Vector2 newPosition)
        {

        }

        public void Move(Vector2 newPosition)
        {

        }

        public void ChangePosition(Vector2 newPosition)
        {

        }

        public void Update()
        {

        }
    }
}
