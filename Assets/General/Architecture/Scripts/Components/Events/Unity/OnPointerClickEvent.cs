using Leopotam.Ecs;
using UnityEngine;

namespace General.Components.Events.Unity
{
    internal struct OnPointerClickEvent
    {
        public GameObject Sender;
        public EcsEntity EntitySender;
    }
}