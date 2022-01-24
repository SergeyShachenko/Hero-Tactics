using General.Components.Events.Unity;
using Leopotam.Ecs;
using UnityEngine;

namespace General.UnityComponents.MonoLinks.Events
{
    public class OnCollisionStayLink : PhysicsLinkBase
    {
        private void OnCollisionStay(Collision other)
        {
            Entity.Get<OnCollisionStayEvent>() = new OnCollisionStayEvent
            {
                Sender = gameObject,
                Collision = other
            };
        }
    }
}