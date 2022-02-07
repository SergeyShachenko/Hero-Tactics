using Leopotam.Ecs;
using UnityEngine;

namespace General.Components.Events.Unity
{
    internal struct OnTriggerEnterEvent
    {
        public GameObject Sender;
        public Collider Collider;
        public EcsEntity SenderEntity;
        public EcsEntity VisitorEntity;
    }
}