using UnityEngine;

namespace General.Components.Events.Unity
{
    internal struct OnCollisionEnterEvent
    {
        public GameObject Sender;
        public Collision Collision;
    }
}