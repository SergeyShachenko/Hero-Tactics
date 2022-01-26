using General.Components.Events.Unity;
using Leopotam.Ecs;
using UnityEngine;

namespace General.UnityComponents.MonoLinks.Events
{
    public class OnTriggerStayLink : PhysicsLinkBase
    {
        private void OnTriggerStay(Collider other)
        {
            var entitySender = gameObject.GetComponent<MonoEntity>().GetEntity();
            var entityVisitor = other.gameObject.GetComponent<MonoEntity>().GetEntity();
            
            
            World.NewEntity().Get<OnTriggerStayEvent>() = new OnTriggerStayEvent
            {
                Sender = gameObject,
                Collider = other,
                EntitySender = entitySender,
                EntityVisitor = entityVisitor
            };
        }
    }
}