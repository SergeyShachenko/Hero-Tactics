using General.Components.Events.Unity;
using Leopotam.Ecs;
using UnityEngine;

namespace General.UnityComponents.MonoLinks.Events
{
    public class OnCollisionEnterLink : PhysicsLinkBase
    {
        private void OnCollisionEnter(Collision other)
        {
            _entity.Get<OnCollisionEnterEvent>() = new OnCollisionEnterEvent
            {
                Sender = gameObject,
                Collision = other
            };
        }
    }
}