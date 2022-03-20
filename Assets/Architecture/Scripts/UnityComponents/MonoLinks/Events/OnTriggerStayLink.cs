using Components.Events.Physics;
using Leopotam.Ecs;
using UnityComponents.MonoLinks.Base;
using UnityEngine;

namespace UnityComponents.MonoLinks.Events
{
    public sealed class OnTriggerStayLink : PhysicsLinkBase
    {
        private void OnTriggerStay(Collider other)
        {
            var entitySender = gameObject.GetComponent<MonoEntity>().GetEntity();
            var entityVisitor = other.gameObject.GetComponent<MonoEntity>().GetEntity();
            
            
            World.NewEntity().Get<OnTriggerStayEvent>() = new OnTriggerStayEvent
            {
                SenderGameObj = gameObject,
                Collider = other,
                Sender = entitySender,
                Visitor = entityVisitor
            };
        }
    }
}