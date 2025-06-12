using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Model.ObjectTypes
{
    public interface IAttackObject
    {
        public bool DestroyPermission { get; }
        public int Damage { get; }
        RectangleCollider Collider { get; }
        void MoveCollider();
    }
}
