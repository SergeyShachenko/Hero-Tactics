using Components.Events.Unity;
using Leopotam.Ecs;
using UnityEngine;

namespace UnityComponents.MonoLinks.Events
{
    public class OnCollisionEnterLink : PhysicsLinkBase
    {
        private void OnCollisionEnter(Collision other)
        {
            var entitySender = gameObject.GetComponent<MonoEntity>().GetEntity();
            var entityVisitor = other.gameObject.GetComponent<MonoEntity>().GetEntity();
            
            
            World.NewEntity().Get<OnCollisionEnterEvent>() = new OnCollisionEnterEvent
            {
                Sender = gameObject,
                Collision = other,
                EntitySender = entitySender,
                EntityVisitor = entityVisitor
            };
        }
    }
}