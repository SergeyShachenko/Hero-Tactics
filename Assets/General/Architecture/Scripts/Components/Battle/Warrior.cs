using System;
using UnityEngine;

namespace General.Components.Battle
{
    [Serializable] public struct Warrior
    {
        public bool isBoss;
        public WarriorType Type;
    }

    public enum WarriorType
    {
        Sword,
        SwordShield
    }
}