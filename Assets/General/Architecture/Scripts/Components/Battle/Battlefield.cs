using System;
using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

namespace General.Components.Battle
{
    [Serializable] public struct Battlefield
    {
        public BattleSide BattleSide;
        public bool IsBoss;
        public List<WarriorType> SpawnOnStart;
        public Transform StandPositions, BattlePositions;
        public List<Transform> ApprovedWays;
        [HideInInspector] public BattlefieldState State;
        [HideInInspector] public List<EcsEntity> Visitors;
    }

    public enum BattlefieldState
    {
        Free, Occupied, Battle
    }
}