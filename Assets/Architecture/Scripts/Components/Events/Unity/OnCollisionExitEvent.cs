using Leopotam.Ecs;
using UnityEngine;

namespace Components.Events.Unity
{
    internal struct OnCollisionExitEvent
    {
        public GameObject Sender;
        public Collision Collision;
        public EcsEntity EntitySender;
        public EcsEntity EntityVisitor;
    }
}