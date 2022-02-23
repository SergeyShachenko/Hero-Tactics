using Leopotam.Ecs;
using UnityEngine;

namespace Components.Events.Physics
{
    internal struct OnTriggerExitEvent
    {
        public GameObject GameObjSender;
        public Collider Collider;
        public EcsEntity Sender;
        public EcsEntity GoneVisitor;
    }
}