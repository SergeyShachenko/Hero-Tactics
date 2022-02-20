﻿using System;
using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

namespace Components.Battle
{
    [Serializable] public struct Battlefield
    {
        [HideInInspector] public BattlefieldState State;
        public bool SpawnBoss;
        public BattleSide SpawnWarriorBattleSide;
        public List<WarriorType> SpawnWarriorOnStart;
        public List<Transform> Ways;
        [HideInInspector] public List<EcsEntity> Visitors;
        [HideInInspector] public Transform StandPoints, BattlePoints;
        [HideInInspector] public Transform Model;
    }

    public enum BattlefieldState
    {
        Free, Occupied, Battle
    }
}