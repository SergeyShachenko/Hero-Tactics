using General.Components.Battle;
using UnityEngine;

namespace General.Components.Events
{
    public struct SpawnWarriorEvent
    {
        public bool IsBoss;
        public BattleSide BattleSide;
        public WarriorType WarriorType;
        public Transform SpawnPoint;
    }
}