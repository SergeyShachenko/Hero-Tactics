using Leopotam.Ecs;
using UnityEngine;

namespace General.Components.Events.Unity
{
    internal struct OnTriggerExitEvent
    {
        public GameObject Sender;
        public Collider Collider;
        public EcsEntity EntitySender;
        public EcsEntity EntityGoneVisitor;
    }
}