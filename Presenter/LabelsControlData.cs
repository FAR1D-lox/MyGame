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
    public class LabelsControlData
    {
        public ButtonState MouseLeftButtonState { get; set; }
        public Vector2 MousePosition { get; set; }
        public bool IsEscPressed { get; set; }
    }
}
