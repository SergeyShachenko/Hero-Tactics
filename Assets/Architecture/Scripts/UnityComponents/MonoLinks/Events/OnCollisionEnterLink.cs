using Components.Events.Physics;
using Leopotam.Ecs;
using UnityComponents.MonoLinks.Base;
using UnityEngine;

namespace UnityComponents.MonoLinks.Events
{
    public sealed class OnCollisionEnterLink : PhysicsLinkBase
    {
        private void OnCollisionEnter(Collision other)
        {
            var entitySender = gameObject.GetComponent<MonoEntity>().GetEntity();
            var entityVisitor = other.gameObject.GetComponent<MonoEntity>().GetEntity();
            
            
            World.NewEntity().Get<OnCollisionEnterEvent>() = new OnCollisionEnterEvent
            {
                SenderGameObj = gameObject,
                Collision = other,
                Sender = entitySender,
                Visitor = entityVisitor
            };
        }
    }
}