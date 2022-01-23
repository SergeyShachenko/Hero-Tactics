using UnityEngine;

namespace General.Components.Events.Unity
{
    internal struct OnTriggerEnterEvent
    {
        public GameObject Sender;
        public Collider Collider;
    }
}