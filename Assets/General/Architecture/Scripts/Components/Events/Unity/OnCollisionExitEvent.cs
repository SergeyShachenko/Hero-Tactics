using Leopotam.Ecs;
using UnityEngine;

namespace General.Components.Events.Unity
{
    internal struct OnCollisionExitEvent
    {
        public GameObject Sender;
        public Collision Collision;
        public EcsEntity EntitySender;
        public EcsEntity EntityVisitor;
    }
}