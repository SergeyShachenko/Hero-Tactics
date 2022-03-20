using Leopotam.Ecs;
using UnityEngine;

namespace Components.Events.Physics
{
    internal struct OnCollisionExitEvent
    {
        public Collision Collision;
        public GameObject SenderGameObj;
        public EcsEntity Sender;
        public EcsEntity Visitor;
    }
}