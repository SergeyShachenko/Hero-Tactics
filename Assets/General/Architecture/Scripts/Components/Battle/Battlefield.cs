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
        public List<Transform> AvailablePositions;
        [HideInInspector] public BattlefieldState State;
        [HideInInspector] public List<EcsEntity> Visitors;
        [HideInInspector] public Transform StandPoints, BattlePoints;
    }

    public enum BattlefieldState
    {
        Free, Occupied, Battle
    }
}