using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyGame.Model
{
    public interface IGravity : IObject
    {
        bool IsGrounded { get; set; }
        float JumpForce { get; }
        float Gravity { get; }
        float VerticalSpeed { get; }

        void JumpAttempt();
        void UpdateGravity();
        void PushTop();
    }
}
