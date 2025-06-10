using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using MyGame.Model.ObjectTypes;

namespace MyGame.Model.Objects.Labels
{
    public class PauseWindow : ILabel
    {
        public int ImageId { get; }
        public Vector2 Pos { get; }
        public int Width { get; }
        public int Height { get; }
        public PauseWindow(Vector2 position, int width, int height, int imageId)
        {
            ImageId = imageId;
            Pos = position;
            Width = width;
            Height = height;
        }


    }
}
