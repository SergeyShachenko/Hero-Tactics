using General.Components.Events.Unity;
using Leopotam.Ecs;
using UnityEngine;

namespace General.UnityComponents.MonoLinks.Events
{
    public class OnTriggerEnterLink : PhysicsLinkBase
    {
        private void OnTriggerEnter(Collider other)
        {
            var entitySender = gameObject.GetComponent<MonoEntity>().GetEntity();
            var entityVisitor = other.gameObject.GetComponent<MonoEntity>().GetEntity();
            
            
            World.NewEntity().Get<OnTriggerEnterEvent>() = new OnTriggerEnterEvent
            {
                Sender = gameObject,
                Collider = other,
                SenderEntity = entitySender,
                VisitorEntity = entityVisitor
            };
            
            //Debug.Log("OnTriggerEnter");
        }
    }
}