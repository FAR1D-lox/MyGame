﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace KnightLegends.Model.ObjectTypes
{
    public interface IAnimationMapObject : IAnimationObject
    {
        public int AnimationTimer { get; }
    }
}
