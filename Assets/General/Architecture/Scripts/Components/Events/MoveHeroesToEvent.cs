using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

namespace General.Components.Events
{
    internal struct MoveHeroesToEvent
    {
        public List<EcsEntity> Heroes;
        public Vector3 TargetPosition;
    }
}