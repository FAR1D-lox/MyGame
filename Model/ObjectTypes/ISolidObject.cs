using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Model.ObjectTypes
{
    public interface ISolidObject : IMapObject
    {
        RectangleCollider Collider { get; }
        void MoveCollider();
    }
}
