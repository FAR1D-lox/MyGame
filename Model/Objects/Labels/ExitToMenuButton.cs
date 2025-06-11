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
using MyGame.Model.ObjectTypes;

namespace MyGame.Model.Objects.Labels
{
    public class ExitToMenuButton : IButton
    {
        public int ImageId { get; }
        private Vector2 _pos;
        public Vector2 Pos
        {
            get
            {
                return _pos + PositionRelative;
            }

            set
            {
                _pos = value;
                Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y, Width, Height);
            }
        }
        public int Width { get; }
        public int Height { get; }
        public bool CursorHover { get; private set; }
        public Vector2 ImagePos { get; private set; }
        private RectangleCollider Collider { get; set; }
        public Vector2 PositionRelative { get; }
        public ExitToMenuButton(Vector2 position, int width, int height, int imageId, Vector2 positionRelative)
        {
            ImageId = imageId;
            PositionRelative = positionRelative;
            Pos = position;
            Width = width;
            Height = height;
            Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y, Width, Height);
        }


        public Rectangle Animate(int widthImage)
        {
            ImagePos = Animation.AnimateButton(Width, CursorHover);
            return new Rectangle((int)ImagePos.X, (int)ImagePos.Y, Width, Height);
        }

        public void CheckCursorHover(Vector2 MousePosition)
        {
            if (RectangleCollider.IsCollided(Collider, MousePosition))
                CursorHover = true;
            else
                CursorHover = false;
        }
    }
}
