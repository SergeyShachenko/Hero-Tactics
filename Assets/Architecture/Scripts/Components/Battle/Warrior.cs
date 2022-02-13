using System;

namespace Components.Battle
{
    public struct Warrior
    {
        public WarriorType Type;
    }

    
    [Serializable] public enum WarriorType
    {
        Sword,
        SwordShield
    }
}