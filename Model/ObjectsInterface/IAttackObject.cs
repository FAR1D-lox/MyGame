﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightLegends.Model.ObjectTypes
{
    public interface IAttackObject : IAnimationMapObject, IMapObject
    {
        public bool DestroyPermission { get; }
        public int Damage { get; }
        RectangleCollider Collider { get; }
        void MoveCollider();
    }
}
