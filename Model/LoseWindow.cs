using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MyGame.Model
{
    public class LoseWindow : ILabel
    {
        public int ImageId { get; }
        public Vector2 Pos { get; }
        public int Width { get; }
        public int Height { get; }
        public LoseWindow(Vector2 position, int width, int height, int imageId)
        {
            ImageId = imageId;
            Pos = position;
            Width = width;
            Height = height;
        }


    }
}
