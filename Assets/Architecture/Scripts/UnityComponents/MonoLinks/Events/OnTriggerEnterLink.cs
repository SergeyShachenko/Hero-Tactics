using Components.Events.Physics;
using Leopotam.Ecs;
using UnityComponents.MonoLinks.Base;
using UnityEngine;

namespace UnityComponents.MonoLinks.Events
{
    public sealed class OnTriggerEnterLink : PhysicsLinkBase
    {
        private void OnTriggerEnter(Collider other)
        {
            var entitySender = gameObject.GetComponent<MonoEntity>().GetEntity();
            var entityVisitor = other.gameObject.GetComponent<MonoEntity>().GetEntity();
            
            
            World.NewEntity().Get<OnTriggerEnterEvent>() = new OnTriggerEnterEvent
            {
                SenderGameObj = gameObject,
                Collider = other,
                Sender = entitySender,
                Visitor = entityVisitor
            };
            
            //Debug.Log("OnTriggerEnter");
        }
    }
}