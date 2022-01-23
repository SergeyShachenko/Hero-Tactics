using General.Components.Events.Unity;
using Leopotam.Ecs;
using UnityEngine;

namespace General.UnityComponents.MonoLinks.Events
{
    public class OnTriggerEnterLink : PhysicsLinkBase
    {
        private void OnTriggerEnter(Collider other)
        {
            _entity.Get<OnTriggerEnterEvent>() = new OnTriggerEnterEvent
            {
                Sender = gameObject,
                Collider = other
            };
        }
    }
}