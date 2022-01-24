using General.Components.Events.Unity;
using Leopotam.Ecs;
using UnityEngine;

namespace General.UnityComponents.MonoLinks.Events
{
    public class OnTriggerStayLink : PhysicsLinkBase
    {
        private void OnTriggerStay(Collider other)
        {
            Entity.Get<OnTriggerStayEvent>() = new OnTriggerStayEvent
            {
                Sender = gameObject,
                Collider = other
            };
        }
    }
}