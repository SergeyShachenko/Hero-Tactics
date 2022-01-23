using General.Components.Events.Unity;
using Leopotam.Ecs;
using UnityEngine;

namespace General.UnityComponents.MonoLinks.Events
{
    public class OnTriggerExitLink : PhysicsLinkBase
    {
        private void OnTriggerExit(Collider other)
        {
            _entity.Get<OnTriggerExitEvent>() = new OnTriggerExitEvent
            {
                Sender = gameObject,
                Collider = other
            };
        }
    }
}