using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

namespace Components.Events.Move
{
    internal struct MoveHeroesToEvent
    {
        public List<EcsEntity> Heroes;
        public Vector3 TargetPosition;
    }
}