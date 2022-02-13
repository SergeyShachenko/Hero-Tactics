using Components.Events.Unity;
using Leopotam.Ecs;
using UnityEngine;

namespace UnityComponents.MonoLinks.Events
{
    public class OnTriggerExitLink : PhysicsLinkBase
    {
        private void OnTriggerExit(Collider other)
        {
            var entitySender = gameObject.GetComponent<MonoEntity>().GetEntity();
            var entityVisitor = other.gameObject.GetComponent<MonoEntity>().GetEntity();
            
            
            World.NewEntity().Get<OnTriggerExitEvent>() = new OnTriggerExitEvent
            {
                Sender = gameObject,
                Collider = other,
                EntitySender = entitySender,
                EntityGoneVisitor = entityVisitor
            };
            
            //Debug.Log("OnTriggerExit");
        }
    }
}