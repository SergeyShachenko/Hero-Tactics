using General.Components.Battle;
using UnityEngine;

namespace General.Components.Events
{
    public struct SpawnWarriorEvent
    {
        public BattleSide BattleSide;
        public WarriorType WarriorType;
        public bool IsBoss;
        public byte SquadID;
        public Transform SpawnPoint;
    }
}