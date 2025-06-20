﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using KnightLegends.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KnightLegends.View
{
    public class InputData : EventArgs
    {
        public Keys[] PressedKeys { get; set; }
        public ButtonState MouseLeftButtonState { get; set; }
        public Vector2 MousePosition { get; set; }
    }
}
