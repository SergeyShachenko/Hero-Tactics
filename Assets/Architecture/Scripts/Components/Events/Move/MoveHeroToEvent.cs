using Leopotam.Ecs;
using UnityEngine;

namespace Components.Events.Move
{
    internal struct MoveHeroToEvent
    {
        public EcsEntity Entity;
        public Vector3 Position;
    }
}