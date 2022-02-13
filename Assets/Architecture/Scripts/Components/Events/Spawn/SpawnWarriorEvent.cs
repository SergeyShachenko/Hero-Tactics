using Components.Battle;
using UnityEngine;

namespace Components.Events.Spawn
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