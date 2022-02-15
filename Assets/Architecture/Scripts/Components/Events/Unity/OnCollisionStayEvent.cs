using Leopotam.Ecs;
using UnityEngine;

namespace Components.Events.Unity
{
    internal struct OnCollisionStayEvent
    {
        public GameObject GameObjSender;
        public Collision Collision;
        public EcsEntity Sender;
        public EcsEntity Visitor;
    }
}