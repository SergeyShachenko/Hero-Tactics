﻿using Leopotam.Ecs;
using UnityEngine;

namespace Components.Events.Physics
{
    internal struct OnPointerClickEvent
    {
        public GameObject SenderGameObj;
        public EcsEntity Sender;
    }
}