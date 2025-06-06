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
    public interface IMapObject : IObject
    {
        Vector2 PrevPos { get; }
        Vector2 Speed { get; }

        public void Move(float xMove, float yMove);
        public void ChangePosition(float xPos, float yPos);
        public void ChangePreviousPosition(float xPos, float yPos);
        public void ChangeSpeed(float xSpeed, float ySpeed);
        public void SpeedUp(float xSpeed, float ySpeed);
        public void Update();
    }
}
