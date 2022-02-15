﻿using Leopotam.Ecs;
using UnityEngine;

namespace Components.Events.Unity
{
    internal struct OnPointerClickEvent
    {
        public GameObject GameObjSender;
        public EcsEntity Sender;
    }
}