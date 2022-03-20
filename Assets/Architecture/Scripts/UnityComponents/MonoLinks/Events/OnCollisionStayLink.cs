using Components.Events.Physics;
using Leopotam.Ecs;
using UnityComponents.MonoLinks.Base;
using UnityEngine;

namespace UnityComponents.MonoLinks.Events
{
    public sealed class OnCollisionStayLink : PhysicsLinkBase
    {
        private void OnCollisionStay(Collision other)
        {
            var entitySender = gameObject.GetComponent<MonoEntity>().GetEntity();
            var entityVisitor = other.gameObject.GetComponent<MonoEntity>().GetEntity();
            
            
            World.NewEntity().Get<OnCollisionStayEvent>() = new OnCollisionStayEvent
            {
                SenderGameObj = gameObject,
                Collision = other,
                Sender = entitySender,
                Visitor = entityVisitor
            };
        }
    }
}