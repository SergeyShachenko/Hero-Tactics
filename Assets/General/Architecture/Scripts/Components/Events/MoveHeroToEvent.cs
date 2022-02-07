using Leopotam.Ecs;
using UnityEngine;

namespace General.Components.Events
{
    internal struct MoveHeroToEvent
    {
        public EcsEntity Entity;
        public Vector3 Position;
    }
}