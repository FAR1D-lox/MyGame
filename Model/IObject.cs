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
    public interface IObject
    {
        int ImageId { get; }
        Vector2 Pos { get; }
        public int Width { get; }
        public int Height { get; }
    }
}
