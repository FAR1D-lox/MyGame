using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyGame.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyGame.Presenter
{
    public class LabelsControlData
    {
        public MouseClick MouseLeftButtonState { get; set; }
        public Vector2 MousePosition { get; set; }
        public bool IsEscPressed { get; set; }
    }
}
