using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using KnightLegends.Model.ObjectTypes;

namespace KnightLegends.Model.Objects.MapObjects
{
    public class Portal : NoSolidObject
    {

        public new int ImageId { get; }
        public new Vector2 Pos { get; set; }
        public new Vector2 PrevPos { get; set; }
        public new Vector2 Speed { get; private set; }
        public new int Width { get; }
        public new int Height { get; }
        public RectangleCollider Collider { get; private set; }

        public Portal(Vector2 position, int width, int height, int imageId) : base(position, width, height, imageId)
        {
            ImageId = imageId;
            PrevPos = position;
            Pos = position;
            Width = width;
            Height = height;
            Collider = new RectangleCollider((int)Pos.X + 11, (int)Pos.Y + 13, Width - 11 * 2, Height - 11 * 2);
        }


    }
}