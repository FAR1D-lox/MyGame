using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using KnightLegends.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KnightLegends.Presenter
{
    public class MainCharacterControlData
    {
        public Direction Direction { get; set; }
        public ButtonState MouseLeftButtonState { get; set; }
    }
}
