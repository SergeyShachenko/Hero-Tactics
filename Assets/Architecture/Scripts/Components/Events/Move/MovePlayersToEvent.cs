using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

namespace Components.Events.Move
{
    internal struct MovePlayersToEvent
    {
        public HashSet<EcsEntity> Players;
        public Vector3 TargetPosition;
    }
}