using General.Components.Events.Unity;
using Leopotam.Ecs;
using UnityEngine;

namespace General.UnityComponents.MonoLinks.Events
{
    public class OnCollisionStayLink : PhysicsLinkBase
    {
        private void OnCollisionStay(Collision other)
        {
            var entitySender = gameObject.GetComponent<MonoEntity>().GetEntity();
            var entityVisitor = other.gameObject.GetComponent<MonoEntity>().GetEntity();
            
            
            World.NewEntity().Get<OnCollisionStayEvent>() = new OnCollisionStayEvent
            {
                Sender = gameObject,
                Collision = other,
                EntitySender = entitySender,
                EntityVisitor = entityVisitor
            };
        }
    }
}