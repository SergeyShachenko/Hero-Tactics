using System;
using System.Collections.Generic;
using UnityEngine;

namespace General.Components.Battle
{
    [Serializable] public struct Battlefield
    {
        public BattleSide BattleSide;
        public bool IsBoss;
        public List<WarriorType> WarriorTypes;
        public Transform StandPositions, BattlePositions;
        public List<Transform> ApprovedWays;
    }
}