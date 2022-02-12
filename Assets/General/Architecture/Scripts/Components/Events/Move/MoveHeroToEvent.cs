using Leopotam.Ecs;
using UnityEngine;

namespace General.Components.Events.Move
{
    internal struct MoveHeroToEvent
    {
        public EcsEntity Entity;
        public Vector3 Position;
    }
}