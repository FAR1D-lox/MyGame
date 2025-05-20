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
        int ImageId { get; set; }
        Vector2 Pos { get; }
        Vector2 PrevPos { get; set; }
        Vector2 Speed { get; set; }
        public int Width { get; }
        public int Height { get; }

        public void Move(Vector2 positionChange);
        public void ChangePosition(Vector2 newPosition);
        public void Update();
    }
}
