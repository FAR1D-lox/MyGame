using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyGame.Model
{
    public interface IAliveObject : IMapObject
    {
        int HP { get; set; }
        int ImmortalTimer { get; }
        void TryReduceHealthPoints(int damage);
        RectangleCollider Collider { get; }
        void MoveCollider();
    }
}
