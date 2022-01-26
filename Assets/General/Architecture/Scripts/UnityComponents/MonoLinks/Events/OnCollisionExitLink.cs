using General.Components.Events.Unity;
using Leopotam.Ecs;
using UnityEngine;

namespace General.UnityComponents.MonoLinks.Events
{
    public class OnCollisionExitLink : PhysicsLinkBase
    {
        private void OnCollisionExit(Collision other)
        {
            var entitySender = gameObject.GetComponent<MonoEntity>().GetEntity();
            var entityVisitor = other.gameObject.GetComponent<MonoEntity>().GetEntity();
            
            
            World.NewEntity().Get<OnCollisionExitEvent>() = new OnCollisionExitEvent
            {
                Sender = gameObject,
                Collision = other,
                EntitySender = entitySender,
                EntityVisitor = entityVisitor
            };
        }
    }
}