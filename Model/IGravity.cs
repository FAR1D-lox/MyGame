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
        bool isGrounded { get; set; }
        float jumpForce { get; set; }
        float gravity { get; set; }
        float verticalSpeed { get; set; }

        void JumpAttempt();
    }
}
