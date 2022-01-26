using Leopotam.Ecs;
using UnityEngine;

namespace General.Components.Events.Unity
{
    internal struct OnTriggerStayEvent
    {
        public GameObject Sender;
        public Collider Collider;
        public EcsEntity EntitySender;
        public EcsEntity EntityVisitor;
    }
}