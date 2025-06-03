using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyGame.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using static MyGame.Model.Direction;
using static MyGame.Model.MouseClick;

namespace MyGame.View
{
    public static class Controller
    {
        private static int ClickTimer = 0;
        public static Direction? KeyBoardController(Keys[] keys)
        {
            if (keys.Length > 0)
            {
                var k1 = keys[0];
                var k2 = new Keys();
                if (keys.Length == 2)
                    k2 = keys[1];

                switch (k1)
                {
                    case Keys.Escape:
                        {
                            return null;
                        }
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


        public static MouseClick MouseController(ButtonState mouseLeftClick)
        {
            if (mouseLeftClick == ButtonState.Pressed && ClickTimer <= 0)
            {
                ClickTimer = 70;
                return pressed;
            }
            ClickTimer -= 1;
            return released;
        }
    }
}
