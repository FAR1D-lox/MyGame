using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Model
{
    public class RectangleCollider
    {
        public Rectangle Boundary { get; set; } //граница
        public RectangleCollider(int x, int y, int width, int height)
        {
            Boundary = new Rectangle(x, y, width, height);
        }

        public static bool IsCollided(RectangleCollider r1, RectangleCollider r2)
        {
            return r1.Boundary.Intersects(r2.Boundary);
        }
        public static bool IsCollided(RectangleCollider r1, Vector2 position)
        {
            var r2 = new RectangleCollider((int)position.X, (int)position.Y, 1, 1);
            return r1.Boundary.Intersects(r2.Boundary);
        }
    }
}
