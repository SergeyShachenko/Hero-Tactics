using System;
using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

namespace Components.Battle
{
    [Serializable] public struct Battlefield
    {
        [Header("Settings")]
        public Vector3 PlacementHeroRotation;
        public Vector3 PlacementEnemyRotation;
        public List<Transform> Ways;
        
        [Header("SpawnOnStart")]
        public bool SpawnBoss;
        public BattleSide SpawnWarriorBattleSide;
        public List<WarriorType> SpawnWarriorsOnStart;

        [HideInInspector] public BattlefieldState State;
        [HideInInspector] public HashSet<EcsEntity> Visitors;
        [HideInInspector] public Transform StandPoints, BattlePoints;
        [HideInInspector] public Transform Model;
    }

    public enum BattlefieldState
    {
        Free, Occupied, Battle
    }
}