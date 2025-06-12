using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Timers;
using static MyGame.Model.Direction;

namespace MyGame.Presenter
{
    public static class Controller
    {
        public static Direction FindDirection(Keys[] keys)
        {
            if (keys.Length > 0)
            {
                var k1 = keys[0];
                var k2 = new Keys();
                if (keys.Length == 2)
                    k2 = keys[1];

                switch (k1)
                {
                    case Keys.Space:
                        {
                            if (k2 == Keys.A)
                                return leftUp;
                            else if (k2 == Keys.D)
                                return rightUp;
                            return up;
                        }
                    case Keys.A:
                        {
                            if (k2 == Keys.Space)
                                return leftUp;
                            else if (k2 == Keys.D)
                                return None;
                            return left;
                        }
                    case Keys.D:
                        {
                            if (k2 == Keys.Space)
                                return rightUp;
                            else if (k2 == Keys.A)
                                return None;
                            return right;
                        }
                }
            }
            return None;
        }

        public static bool IsPressedESC(Keys[] keys)
        {
            if (keys.Contains(Keys.Escape))
                return true;
            return false;
        }
    }
}
