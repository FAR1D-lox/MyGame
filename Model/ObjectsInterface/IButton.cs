using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text;
using System.Threading.Tasks;

namespace KnightLegends.Model.ObjectTypes
{
    public interface IButton : IObject, IAnimationObject
    {
        public void CheckCursorHover(Vector2 MousePosition);
        public Vector2 PositionRelative { get; }
        public bool CursorHover { get; }
        public new Vector2 Pos { get; set; }
    }
}
