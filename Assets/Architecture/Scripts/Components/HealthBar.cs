using System;
using UnityEngine;
using UnityEngine.UI;

namespace Components
{
    [Serializable] public struct HealthBar
    {
        public Transform Canvas;
        public Image Frame, Bar, Background;
    }
}