using Components.Events.Physics;
using Leopotam.Ecs;
using UnityEngine;

namespace UnityComponents.MonoLinks.Events
{
    public class OnCollisionStayLink : PhysicsLinkBase
    {
        private void OnCollisionStay(Collision other)
        {
            var entitySender = gameObject.GetComponent<MonoEntity>().GetEntity();
            var entityVisitor = other.gameObject.GetComponent<MonoEntity>().GetEntity();
            
            
            World.NewEntity().Get<OnCollisionStayEvent>() = new OnCollisionStayEvent
            {
                GameObjSender = gameObject,
                Collision = other,
                Sender = entitySender,
                Visitor = entityVisitor
            };
        }
    }
}