using Components.Events.Physics;
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
                GameObjSender = gameObject,
                Collision = other,
                Sender = entitySender,
                Visitor = entityVisitor
            };
        }
    }
}