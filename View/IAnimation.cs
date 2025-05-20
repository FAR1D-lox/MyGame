using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyGame.Model;

namespace MyGame.View
{
    public interface IAnimation : IObject
    {
        public int timer { get; }
        public Vector2 ImagePos { get; }
        public Rectangle? Animate(int heightImage, int widthImage);
    }
}
