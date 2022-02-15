using Components.Events.Unity;
using Leopotam.Ecs;
using UnityEngine;

namespace UnityComponents.MonoLinks.Events
{
    public class OnTriggerStayLink : PhysicsLinkBase
    {
        private void OnTriggerStay(Collider other)
        {
            var entitySender = gameObject.GetComponent<MonoEntity>().GetEntity();
            var entityVisitor = other.gameObject.GetComponent<MonoEntity>().GetEntity();
            
            
            World.NewEntity().Get<OnTriggerStayEvent>() = new OnTriggerStayEvent
            {
                GameObjSender = gameObject,
                Collider = other,
                Sender = entitySender,
                Visitor = entityVisitor
            };
        }
    }
}