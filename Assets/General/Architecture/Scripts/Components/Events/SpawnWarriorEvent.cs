using General.Components.Battle;
using UnityEngine;

namespace General.Components.Events
{
    internal struct SpawnWarriorEvent
    {
        public BattleSide BattleSide;
        public WarriorType WarriorType;
        public bool IsBoss;
        public int SquadID;
        public Transform SpawnPoint;
    }
}