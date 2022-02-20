using System;
using UnityEngine;
using UnityEngine.UI;

namespace Components
{
    [Serializable] public struct HealthBar
    {
        public Image Frame;
        public Image Bar;
        [HideInInspector] public float StartHealth, CurrentHealth;
    }
}