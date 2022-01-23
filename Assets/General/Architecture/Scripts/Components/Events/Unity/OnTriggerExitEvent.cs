using UnityEngine;

namespace General.Components.Events.Unity
{
    internal struct OnTriggerExitEvent
    {
        public GameObject Sender;
        public Collider Collider;
    }
}