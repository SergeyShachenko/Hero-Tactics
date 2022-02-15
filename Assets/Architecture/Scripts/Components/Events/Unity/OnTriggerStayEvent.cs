using Leopotam.Ecs;
using UnityEngine;

namespace Components.Events.Unity
{
    internal struct OnTriggerStayEvent
    {
        public GameObject GameObjSender;
        public Collider Collider;
        public EcsEntity Sender;
        public EcsEntity Visitor;
    }
}