using General.Components.Events.Unity;
using Leopotam.Ecs;
using UnityEngine;

namespace General.UnityComponents.MonoLinks.Events
{
    public class OnTriggerExitLink : PhysicsLinkBase
    {
        private void OnTriggerExit(Collider other)
        {
            Entity.Get<OnTriggerExitEvent>() = new OnTriggerExitEvent
            {
                Sender = gameObject,
                Collider = other
            };
        }
    }
}