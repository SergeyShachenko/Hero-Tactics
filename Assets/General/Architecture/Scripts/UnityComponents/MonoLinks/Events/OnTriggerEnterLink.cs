using General.Components.Events.Unity;
using Leopotam.Ecs;
using UnityEngine;

namespace General.UnityComponents.MonoLinks.Events
{
    public class OnTriggerEnterLink : PhysicsLinkBase
    {
        private void OnTriggerEnter(Collider other)
        {
            Entity.Get<OnTriggerEnterEvent>() = new OnTriggerEnterEvent
            {
                Sender = gameObject,
                Collider = other
            };
        }
    }
}