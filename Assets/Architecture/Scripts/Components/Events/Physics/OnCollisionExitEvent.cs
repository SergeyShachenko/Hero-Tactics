using Leopotam.Ecs;
using UnityEngine;

namespace Components.Events.Physics
{
    internal struct OnCollisionExitEvent
    {
        public GameObject GameObjSender;
        public Collision Collision;
        public EcsEntity Sender;
        public EcsEntity Visitor;
    }
}