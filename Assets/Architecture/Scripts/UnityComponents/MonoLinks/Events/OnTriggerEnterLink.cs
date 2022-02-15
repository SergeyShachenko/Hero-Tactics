using Components.Events.Unity;
using Leopotam.Ecs;
using UnityEngine;

namespace UnityComponents.MonoLinks.Events
{
    public class OnTriggerEnterLink : PhysicsLinkBase
    {
        private void OnTriggerEnter(Collider other)
        {
            var entitySender = gameObject.GetComponent<MonoEntity>().GetEntity();
            var entityVisitor = other.gameObject.GetComponent<MonoEntity>().GetEntity();
            
            
            World.NewEntity().Get<OnTriggerEnterEvent>() = new OnTriggerEnterEvent
            {
                GameObjSender = gameObject,
                Collider = other,
                Sender = entitySender,
                Visitor = entityVisitor
            };
            
            //Debug.Log("OnTriggerEnter");
        }
    }
}