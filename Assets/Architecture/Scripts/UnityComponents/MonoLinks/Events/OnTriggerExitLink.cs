using Components.Events.Physics;
using Leopotam.Ecs;
using UnityComponents.MonoLinks.Base;
using UnityEngine;

namespace UnityComponents.MonoLinks.Events
{
    public sealed class OnTriggerExitLink : PhysicsLinkBase
    {
        private void OnTriggerExit(Collider other)
        {
            var entitySender = gameObject.GetComponent<MonoEntity>().GetEntity();
            var entityVisitor = other.gameObject.GetComponent<MonoEntity>().GetEntity();
            
            
            World.NewEntity().Get<OnTriggerExitEvent>() = new OnTriggerExitEvent
            {
                SenderGameObj = gameObject,
                Collider = other,
                Sender = entitySender,
                GoneVisitor = entityVisitor
            };
            
            //Debug.Log("OnTriggerExit");
        }
    }
}