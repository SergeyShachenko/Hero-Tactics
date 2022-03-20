using Leopotam.Ecs;
using UnityEngine;

namespace Components.Events.Physics
{
    internal struct OnTriggerStayEvent
    {
        public Collider Collider;
        public GameObject SenderGameObj;
        public EcsEntity Sender;
        public EcsEntity Visitor;
    }
}