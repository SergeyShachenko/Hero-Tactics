using Leopotam.Ecs;
using UnityEngine;

namespace Components.Events.Move
{
    internal struct MovePlayerToEvent
    {
        public EcsEntity Entity;
        public Vector3 TargetPosition;
    }
}