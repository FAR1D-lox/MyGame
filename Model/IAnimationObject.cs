using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyGame.Model
{
    public interface IAnimationObject : IObject
    {
        public int AnimationTimer { get; }
        public Vector2 ImagePos { get; }
        public Rectangle? Animate(int widthImage);
    }
}
