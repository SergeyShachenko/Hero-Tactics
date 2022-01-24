﻿using General.Components.Events.Unity;
using Leopotam.Ecs;
using UnityEngine;

namespace General.UnityComponents.MonoLinks.Events
{
    public class OnCollisionExitLink : PhysicsLinkBase
    {
        private void OnCollisionExit(Collision other)
        {
            Entity.Get<OnCollisionExitEvent>() = new OnCollisionExitEvent
            {
                Sender = gameObject,
                Collision = other
            };
        }
    }
}