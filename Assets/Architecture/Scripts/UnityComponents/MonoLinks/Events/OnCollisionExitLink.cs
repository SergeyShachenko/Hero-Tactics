using Components.Events.Physics;
using Leopotam.Ecs;
using UnityEngine;

namespace UnityComponents.MonoLinks.Events
{
    public class OnCollisionExitLink : PhysicsLinkBase
    {
        private void OnCollisionExit(Collision other)
        {
            var entitySender = gameObject.GetComponent<MonoEntity>().GetEntity();
            var entityVisitor = other.gameObject.GetComponent<MonoEntity>().GetEntity();
            
            
            World.NewEntity().Get<OnCollisionExitEvent>() = new OnCollisionExitEvent
            {
                GameObjSender = gameObject,
                Collision = other,
                Sender = entitySender,
                Visitor = entityVisitor
            };
        }
    }
}