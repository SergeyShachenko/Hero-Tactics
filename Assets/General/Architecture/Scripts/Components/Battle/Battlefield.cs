﻿using System;
using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

namespace General.Components.Battle
{
    [Serializable] public struct Battlefield
    {
        [HideInInspector] public BattlefieldState State;
        public bool SpawnBoss;
        public BattleSide WarriorBattleSide;
        public List<WarriorType> SpawnWarriorOnStart;
        public List<Transform> AvailablePositions;
        [HideInInspector] public List<EcsEntity> Visitors;
        [HideInInspector] public Transform StandPoints, BattlePoints;
    }

    public enum BattlefieldState
    {
        Free, Occupied, Battle
    }
}